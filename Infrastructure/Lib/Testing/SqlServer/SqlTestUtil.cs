using System;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Conventions;
using Lucid.Infrastructure.Lib.Domain.SqlServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace Lucid.Infrastructure.Lib.Testing.SqlServer
{
    public class SqlTestUtil
    {
        public static Schema UpdateMigrations<TFromMigrationAssembly>(string schemaName, string migrationsSourceFolder)
        {
            // if everything is up to date, we should get the schema without touching the DB
            var serverConfig = GetServerConfig();
            var schema = GetSchema(serverConfig, schemaName);

            var testStartTime = DateTime.UtcNow;
            var flagFile = Path.Combine(TestUtil.ProjectPath(), "bin/dbMigrated.flg");

            var skipDbMigration = !Directory.GetCurrentDirectory().Contains("__Instrumented")
                && File.Exists(flagFile)
                && File.GetLastWriteTimeUtc(flagFile) > LastFileUpdateUtc(migrationsSourceFolder);

            if (!skipDbMigration)
                UpdateMigrations<TFromMigrationAssembly>(serverConfig, schema);

            File.WriteAllText(flagFile, "");
            File.SetLastWriteTimeUtc(flagFile, testStartTime);

            return schema;
        }

        private static void UpdateMigrations<TFromMigrationAssembly>(Domain.SqlServer.SqlServer serverConfig, Schema schema)
        {
            serverConfig.CreateSchemas(true, schema);
            DropAll(schema);

            var serviceCollection = new ServiceCollection();
            var serviceProvider = serviceCollection
                .AddFluentMigratorCore()
                .ConfigureRunner(mrb =>
                {
                    mrb.AddSqlServer2016();
                    mrb.ScanIn(typeof(TFromMigrationAssembly).Assembly).For.All();
                    mrb.WithGlobalConnectionString(schema.ConnectionString);
                    mrb.WithVersionTable(new SchemaVersionMetadata(schema.Name));
                })
                .AddScoped<IConventionSet>(sp => new DefaultConventionSet(schema.Name, null))
                .AddScoped<DbQuery>(sp => new DbQuery(schema.ConnectionString, schema.Name))
                .AddLogging(lb =>
                {
                    lb.SetMinimumLevel(LogLevel.Trace);
                    lb.Services.AddSingleton<ILoggerProvider, MigrationUtil.NUnitLoggerProvider>();
                })
                .BuildServiceProvider(false);

            RunMigrations(serviceProvider);
            RunMigrations(serviceProvider);
        }

        private static DateTime LastFileUpdateUtc(string folder)
        {
            var allFiles = Directory.GetFiles(folder, "*.*", SearchOption.AllDirectories);

            if (allFiles.Length == 0)
                return DateTime.MinValue;

            return allFiles.Select(f => File.GetLastWriteTimeUtc(f)).Max();
        }

        private static Domain.SqlServer.SqlServer GetServerConfig()
        {
            var config = TestUtil.GetConfig();

            var sqlServerConfig = config.GetSection("Host").GetSection("SqlServer");

            var sqlServer = new Domain.SqlServer.SqlServer(
                server: sqlServerConfig.GetValue<string>("Server"),
                dbName: sqlServerConfig.GetValue<string>("DbName"),
                userId: sqlServerConfig.GetValue<string>("userId"),
                password: sqlServerConfig.GetValue<string>("password"));

            return sqlServer;
        }

        private static Schema GetSchema(Domain.SqlServer.SqlServer serverConfig, string schemaName)
        {
            var schema = new Schema { Name = schemaName };
            serverConfig.SetSchemaConnections(schema);

            return schema;
        }

        private static void RunMigrations(ServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
                runner.MigrateUp();
            }
        }

        private static Schema DropAll(Schema schema)
        {
            TestContext.Progress.WriteLine($"Dropping all items for schema {schema.Name}");

            using (var cn = new SqlConnection(schema.ConnectionString))
            {
                cn.Open();
                DropAllItems(cn);
            }

            return schema;
        }

        private static void DropAllItems(SqlConnection cn)
        {
            using (var cmd = cn.CreateCommand())
            {
                // https://stackoverflow.com/a/8150428/357728

                var sql = @"
                    declare @n char(1)
                    set @n = char(10)
                    declare @stmt nvarchar(max)

                    -- procedures
                    select @stmt = isnull( @stmt + @n, '' ) +
                        'drop procedure [' + schema_name(schema_id) + '].[' + name + ']'
                    from sys.procedures

                    -- check constraints
                    select @stmt = isnull( @stmt + @n, '' ) +
                        'alter table [' + schema_name(schema_id) + '].[' + object_name( parent_object_id ) + ']    drop constraint [' + name + ']'
                    from sys.check_constraints

                    -- functions
                    select @stmt = isnull( @stmt + @n, '' ) +
                        'drop function [' + schema_name(schema_id) + '].[' + name + ']'
                    from sys.objects
                    where type in ( 'FN', 'IF', 'TF' )

                    -- views
                    select @stmt = isnull( @stmt + @n, '' ) +
                        'drop view [' + schema_name(schema_id) + '].[' + name + ']'
                    from sys.views

                    -- foreign keys
                    select @stmt = isnull( @stmt + @n, '' ) +
                        'alter table [' + schema_name(schema_id) + '].[' + object_name( parent_object_id ) + '] drop constraint [' + name + ']'
                    from sys.foreign_keys

                    -- tables
                    select @stmt = isnull( @stmt + @n, '' ) +
                        'drop table [' + schema_name(schema_id) + '].[' + name + ']'
                    from sys.tables

                    -- user defined types
                    select @stmt = isnull( @stmt + @n, '' ) +
                        'drop type [' + schema_name(schema_id) + '].[' + name + ']'
                    from sys.types
                    where is_user_defined = 1

                    exec sp_executesql @stmt";

                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
            }
        }
    }
}
