using Todo_API.StartupConfig;

var builder = WebApplication.CreateBuilder(args);

builder.AddStandedServices();
builder.AddAuthServices();
builder.AddHealthCheckServices();
builder.AddCustomServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health").AllowAnonymous();

app.Run();
