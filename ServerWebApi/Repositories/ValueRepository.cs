using Dapper;
using Microsoft.Extensions.Options;
using Npgsql;
using ServerWebApi.Models;
using System.Data;

namespace ServerWebApi.Repositories
{
    internal class ValueRepository : IValueRepository
    {
        private readonly string _connnectionString;
        public ValueRepository(IOptions<PostgresConfiguration> configuration)
        {
            _connnectionString = configuration.Value.DefaultConnection ?? throw new Exception($"Connection string not found.");
        }

        private IDbConnection OpenConnection()
        {
            var connection = new NpgsqlConnection(_connnectionString);
            connection.Open();
            return connection;
        }

        public async Task<IEnumerable<string>> GetValues()
        {
            using var conn = OpenConnection();
            var sql = @"select * from test_data";
            var values = await conn.QueryAsync<TestData>(sql);
            return values.Select(v => v.Value);
        }
    }
}
