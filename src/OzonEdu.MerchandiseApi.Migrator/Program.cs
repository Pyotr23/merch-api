using System.IO;
using System.Linq;
using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace OzonEdu.MerchandiseApi.Migrator
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var connectionString = GetConfiguration()
                .GetSection("DatabaseConnectionOptions:ConnectionString")
                .Get<string>();

            var services = GetFilledServices(connectionString);
            var serviceProvider = services.BuildServiceProvider(false);

            using (serviceProvider.CreateScope())
            {
                var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
                
                if (args.Contains("--dryrun"))
                    runner.ListMigrations();
                else
                    runner.MigrateUp();

                using var connection = new NpgsqlConnection(connectionString);
                connection.Open();
                connection.ReloadTypes();
            }
        }

        private static IConfigurationRoot GetConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();
        }

        private static IServiceCollection GetFilledServices(string connectionString)
        {
            return new ServiceCollection()
                .AddFluentMigratorCore()
                .ConfigureRunner(
                    rb => rb
                        .AddPostgres()
                        .WithGlobalConnectionString(connectionString)
                        .ScanIn(typeof(Program).Assembly)
                        .For.Migrations())
                .AddLogging(lb => lb.AddFluentMigratorConsole());
        }
    }
}