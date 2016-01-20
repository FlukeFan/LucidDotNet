using System.Data.SqlClient;
using FluentMigrator.Infrastructure;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Announcers;
using FluentMigrator.Runner.Generators.SqlServer;
using FluentMigrator.Runner.Initialization;
using FluentMigrator.Runner.Processors;
using FluentMigrator.Runner.Processors.SqlServer;

namespace Demo.Database
{
    public static class DemoMigrationRunner
    {
        public static void Run(string connectionString)
        {
            var firstMigration = typeof(Demo.Database.Migrations.Y2016.M01.V01);
            var assembly = new SingleAssembly(firstMigration.Assembly);

            var migrationGenerator = new SqlServer2008Generator();
            var announcer = new NullAnnouncer();
            var options = new ProcessorOptions();
            var dbFactory = new SqlServerDbFactory();

            var runnerContext = new RunnerContext(announcer)
            {
                Database = "SqlServer2008",
                Connection = connectionString,
                Targets = new string[] { firstMigration.Assembly.FullName },
                NestedNamespaces = true,
                Namespace = "Demo.Database.Migrations",
            };

            using (var connection = new SqlConnection(connectionString))
            using (var processor = new SqlServerProcessor(connection, migrationGenerator, announcer, options, dbFactory))
            {
                var runner = new MigrationRunner(assembly, runnerContext, processor);

                runner.MigrateUp();
            }
        }
    }
}
