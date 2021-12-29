using Microsoft.Extensions.Configuration;

namespace DataMigration.Configuration
{
    internal class MigrationConfiguration : IMigrationConfiguration
    {
        private readonly IConfiguration _configuration;
        public string ConnectionString => _configuration.GetConnectionString("DefaultConnection");
        public int RetryCount => _configuration.GetSection("Migration").GetValue<int>("RetryCount");
        public TimeSpan RetryDelayDuration => _configuration.GetSection("Migration").GetValue<TimeSpan>("RetryDelaySeconds");

        public MigrationConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }
    }
}
