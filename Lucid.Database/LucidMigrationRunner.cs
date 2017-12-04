﻿using System.Data.SqlClient;
using FluentMigrator.Infrastructure;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Announcers;
using FluentMigrator.Runner.Generators.SqlServer;
using FluentMigrator.Runner.Initialization;
using FluentMigrator.Runner.Processors;
using FluentMigrator.Runner.Processors.SqlServer;

namespace Lucid.Database
{
    public static class LucidMigrationRunner
    {
        public static void Run(string connectionString)
        {
            var firstMigration = typeof(Migrations.V001.V000.Build000);
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
                Namespace = "Lucid.Database.Migrations",
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
