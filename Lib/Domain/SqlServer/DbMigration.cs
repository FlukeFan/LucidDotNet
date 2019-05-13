using System.Reflection;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Conventions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Lucid.Lib.Domain.SqlServer
{
    public static class DbMigration
    {
        public static void Migrate<TFromMigrationAssembly>(Schema schema, ILogger<MigrationRunner> logger)
        {
            logger.LogInformation($"Migration start for Schema {schema.Name}");

            var assembly = typeof(TFromMigrationAssembly).Assembly;
            var serviceCollection = new ServiceCollection();

            var serviceProvider = serviceCollection
                .AddSingleton(logger)
                .AddMigration(schema, assembly)
                .BuildServiceProvider(false);

            using (var scope = serviceProvider.CreateScope())
            {
                var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
                runner.MigrateUp();
            }

            logger.LogInformation($"Migration Complete for Schema {schema.Name} using migrations in {assembly}");
        }

        public static IServiceCollection AddMigration(this IServiceCollection services, Schema schema, Assembly assembly)
        {
            return services
                .AddFluentMigratorCore()
                .ConfigureRunner(mrb =>
                {
                    mrb.AddSqlServer2016();
                    mrb.ScanIn(assembly).For.All();
                    mrb.WithGlobalConnectionString(schema.ConnectionString);
                    mrb.WithVersionTable(new SchemaVersionMetadata(schema.Name));
                })
                .AddScoped<IConventionSet>(sp => new DefaultConventionSet(schema.Name, null))
                .AddScoped(sp => new DbQuery(schema.ConnectionString, schema.Name));
        }
    }
}
