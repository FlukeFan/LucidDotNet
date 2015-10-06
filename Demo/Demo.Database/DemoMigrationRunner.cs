using System;
using System.Data.SqlClient;
using FluentMigrator;
using FluentMigrator.Infrastructure;
using FluentMigrator.Runner.Announcers;
using FluentMigrator.Runner.Generators.SqlServer;
using FluentMigrator.Runner.Processors;
using FluentMigrator.Runner.Processors.SqlServer;

namespace Demo.Database
{
    public static class DemoMigrationRunner
    {
        public static void Run(string connectionString)
        {
            var firstMigration = typeof(Demo.Database.Migrations.V01);
            var assembly = new SingleAssembly(firstMigration.Assembly);

            var migrationGenerator = new SqlServer2008Generator();
            var announcer = new NullAnnouncer();
            var dbFactory = new SqlServerDbFactory();

            var options = new ProcessorOptions();

            using (var connection = new SqlConnection(connectionString))
            using (var processor = new SqlServerProcessor(connection, migrationGenerator, announcer, options, dbFactory))
                Execute(firstMigration, processor, assembly, connectionString);
        }

        private static void Execute(Type migrationType, IMigrationProcessor processor, IAssemblyCollection assembly, string connectionString)
        {
            var migration = (IMigration)Activator.CreateInstance(migrationType);

            var conventions = new MigrationConventions();
            var context = new MigrationContext(conventions, processor, assembly, null, connectionString);

            migration.GetUpExpressions(context);

            foreach (var expression in context.Expressions)
                expression.ExecuteWith(processor);
        }
    }
}
