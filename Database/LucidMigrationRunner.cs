using System;
using System.Data.SqlClient;
using FluentMigrator;
using FluentMigrator.Infrastructure;
using FluentMigrator.Runner.Announcers;
using FluentMigrator.Runner.Generators.SqlServer;
using FluentMigrator.Runner.Processors;
using FluentMigrator.Runner.Processors.SqlServer;

namespace Lucid.Database
{
    public static class LucidMigrationRunner
    {
        public static void Run(string connectionString)
        {
            var firstMigration = typeof(Lucid.Database.Migrations.V2015.V01);
            var assembly = new SingleAssembly(firstMigration.Assembly);

            using (var connection = new SqlConnection(connectionString))
            {
                using (var processor = new SqlServerProcessor(connection, new SqlServer2008Generator(), new NullAnnouncer(), new ProcessorOptions(), new SqlServerDbFactory()))
                    Execute(firstMigration, processor, assembly, connectionString);
            }

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
