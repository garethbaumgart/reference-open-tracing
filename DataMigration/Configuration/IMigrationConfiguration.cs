namespace DataMigration.Configuration
{
    internal interface IMigrationConfiguration
    {
        public string ConnectionString { get; }
        public int RetryCount { get; }
        public TimeSpan RetryDelayDuration { get; }
    }
}
