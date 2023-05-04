using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using TodoLibrary.DataAccess;

namespace Todo_API.StartupConfig
{
    public static class DependencyInjectionExtensions
    {
        public static void AddStandedServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.AddSwaggerServices();
        }

    private static void AddSwaggerServices(this WebApplicationBuilder builder)
    {
        var securityScheme = new OpenApiSecurityScheme()
        {
            Name = "Authorization",
            Description = "JWT Authorization header info using bearer tokens",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT"
        };

        var securityRequirement = new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "bearerAuth"
                    }
                },
                new string[] { }
            }
        };

        builder.Services.AddSwaggerGen(opts =>
        {
            opts.AddSecurityDefinition("bearerAuth", securityScheme);
            opts.AddSecurityRequirement(securityRequirement);
        });
    }

        public static void AddCustomServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton<ISqlDataAccess, SQLDataAccess>();
            builder.Services.AddSingleton<ITodoData, TodoData>();
        }

        public static void AddHealthCheckServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddHealthChecks().AddSqlServer(builder.Configuration.GetConnectionString("Default"));
        }

        public static void AddAuthServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddAuthorization(opts =>
            {
                opts.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
            });

            builder.Services.AddAuthentication("Bearer").AddJwtBearer(opts =>
            {
                opts.TokenValidationParameters = new()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration.GetValue<string>("Authentication:Issuer"),
                    ValidAudience = builder.Configuration.GetValue<string>("Authentication:Audience"),
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(
                            builder.Configuration.GetValue<string>("Authentication:SecretKey")))
                };
            });
        }
    }
}