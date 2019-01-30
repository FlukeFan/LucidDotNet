using System;
using System.Data.SqlClient;
using System.IO;
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
        public static void UpdateMigrations<TFromMigrationAssembly>(string schemaName)
        {
            // need to figure out if anything has changed, and if it hasn't, skip remainder of the DB setup
            var schema = GetSchema(schemaName);
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

        private static Schema GetSchema(string schemaName)
        {
            var searchFile = "Infrastructure/Host/web.config.xml";
            var cd = Directory.GetCurrentDirectory();
            var configFile = Path.Combine(cd, searchFile);

            while (!File.Exists(configFile))
            {
                var parent = Directory.GetParent(cd)?.FullName;

                if (parent == cd || parent == null)
                    throw new Exception($"{searchFile} not found in parent of {Directory.GetCurrentDirectory()}");

                cd = parent;
                configFile = Path.Combine(cd, searchFile);
            }

            var config = new ConfigurationBuilder()
                .AddXmlFile(configFile)
                .AddEnvironmentVariables()
                .Build();

            var sqlServerConfig = config.GetSection("Host").GetSection("SqlServer");

            var sqlServer = new Domain.SqlServer.SqlServer(
                server: sqlServerConfig.GetValue<string>("Server"),
                dbName: sqlServerConfig.GetValue<string>("DbName"),
                userId: sqlServerConfig.GetValue<string>("userId"),
                password: sqlServerConfig.GetValue<string>("password"));

            var schema = new Schema { Name = schemaName };
            sqlServer.Init(true, schema);

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
