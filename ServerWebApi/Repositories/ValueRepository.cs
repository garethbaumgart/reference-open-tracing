using Microsoft.Extensions.Options;
using Npgsql;
using ServerWebApi.Models;
using System.Data;
using Dapper;
using System.Diagnostics;

namespace ServerWebApi.Repositories
{
    internal class ValueRepository : IValueRepository
    {
        private readonly string _connnectionString;
        private readonly ActivitySource _activitySource;

        public ValueRepository(IOptions<PostgresConfiguration> configuration, ActivitySource activitySource)
        {
            _connnectionString = configuration.Value.DefaultConnection ?? throw new Exception($"Connection string not found.");
            _activitySource = activitySource;
        }

        private IDbConnection OpenConnection()
        {
            var connection = new NpgsqlConnection(_connnectionString);
            connection.Open();
            return connection;
        }

        public async Task<IEnumerable<string>> GetValues()
        {
            var sql = @"select * from test_data";
            var values = await QueryWithMetrics<TestData>(sql);
            return values.Select(v => v.Value);
        }

        private async Task<IEnumerable<T>> QueryWithMetrics<T>(string sql)
        {
            using var activity = _activitySource.StartActivity("Postgres Database", ActivityKind.Client);
            using var conn = OpenConnection();
            activity?.SetTag("Database", conn.Database);
            activity?.SetTag("ConnectionTimeout", conn.ConnectionTimeout);
            activity?.SetTag("State", conn.State);
            activity?.SetTag("SQL", sql);

            return await conn.QueryAsync<T>(sql);
        }
    }
}
