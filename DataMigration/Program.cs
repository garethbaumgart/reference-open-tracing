using DataMigration.Configuration;
using DataMigration.DBDeployer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

var container = new ServiceCollection();
container.AddLogging(builder => builder.AddConsole());
container.AddSingleton<IConfiguration>(configuration);
container.AddSingleton<IMigrationConfiguration, MigrationConfiguration>();
container.AddTransient<IDBDeployer, PostgresDeployer>();

container
    .BuildServiceProvider()
    .GetRequiredService<IDBDeployer>()
    .Deploy();

