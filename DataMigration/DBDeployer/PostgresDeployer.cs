using DataMigration.Configuration;
using DbUp;
using Microsoft.Extensions.Logging;
using Npgsql;
using Polly;
using System.Net.Sockets;
using System.Reflection;

namespace DataMigration.DBDeployer
{
    internal class PostgresDeployer : IDBDeployer
    {
        private readonly ILogger<PostgresDeployer> _logger;
        private readonly IMigrationConfiguration _configuration;

        public PostgresDeployer(IMigrationConfiguration configuration, ILogger<PostgresDeployer> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public void Deploy() =>
            Policy
                .Handle<SocketException>()
                .Or<NpgsqlException>()
                .WaitAndRetry(
                    _configuration.RetryCount,
                    currentAttemptNumber =>
                    {
                        _logger.LogError(
                            "Migration failed. Current attempt {Attempt}, retrying in {RetryDelay} seconds",
                            currentAttemptNumber,
                            _configuration.RetryDelayDuration.Seconds);

                        return _configuration.RetryDelayDuration;
                    })
                .Execute(() =>
                {
                    EnsureDatabase.For
                        .PostgresqlDatabase(_configuration.ConnectionString);

                    DeployChanges.To
                        .PostgresqlDatabase(_configuration.ConnectionString)
                        .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                        .Build()
                        .PerformUpgrade();
                });
    }
}
