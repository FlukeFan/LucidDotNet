using System;
using System.Data.SqlClient;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Conventions;
using Lucid.Infrastructure.Lib.Domain.SqlServer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace Lucid.Infrastructure.Lib.Testing.SqlServer
{
    public class SqlTestUtil
    {
        public static void UpdateMigrations<TFromMigrationAssembly>(Schema schema)
        {
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
                .AddLogging(lb =>
                {
                    lb.SetMinimumLevel(LogLevel.Trace);
                    lb.Services.AddSingleton<ILoggerProvider, MigrationUtil.NUnitLoggerProvider>();
                })
                .BuildServiceProvider(false);

            RunMigrations(serviceProvider);
            RunMigrations(serviceProvider);
        }

        private static void RunMigrations(ServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
                runner.MigrateUp();
            }
        }

        public static Schema DropAll(string schemaName)
        {
            var dbConfig = Domain.SqlServer.SqlServer.GetConfig(true, Environment.GetEnvironmentVariable("IsRunningInAppVeyor") != null);
            var schema = new Schema { Name = schemaName };
            dbConfig.CreateDb(schema);

            TestContext.Progress.WriteLine($"Dropping all items for schema {schemaName}");

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
