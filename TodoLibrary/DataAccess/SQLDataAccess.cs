using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Dapper;
using System.Data.SqlClient;
using System.Data;

namespace TodoLibrary.DataAccess
{
    public class SQLDataAccess : ISqlDataAccess
    {
        private readonly IConfiguration _config;

        public SQLDataAccess(IConfiguration config)
        {
            _config = config;
        }

    public async Task<List<T>> LoadData<T, U>(
        string storedProcedure, U parameters, string connectionStringName)
    {
        string connectionString = _config.GetConnectionString(connectionStringName);
        using IDbConnection connection = new SqlConnection(connectionString);
        var rows = await connection.QueryAsync<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
        return rows.ToList();
    }

        public async Task SaveData<T>(string storeProcedure, T parameters, string connectionStringName)
        {
            string connectionString = _config.GetConnectionString(connectionStringName);
            using IDbConnection connection = new SqlConnection(connectionString);

            await connection.ExecuteAsync(storeProcedure, parameters, commandType: CommandType.StoredProcedure);
  
        }
    }
}