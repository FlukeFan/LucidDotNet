using System;
using System.Linq;
using Lucid.Domain.Persistence;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Connection;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;

namespace Lucid.Persistence.NHibernate
{
    public static class NhHelper
    {
        public static HbmMapping CreateMappings<TId>(Type rootEntityType)
        {
            var mapper = new ConventionModelMapper();

            var baseEntities =
                rootEntityType.Assembly.GetTypes()
                    .Where(t => t.BaseType == rootEntityType)
                    .OrderBy(t => t.FullName);

            var allEntities =
                rootEntityType.Assembly.GetTypes()
                    .Where(t => rootEntityType.IsAssignableFrom(t) && t != rootEntityType)
                    .OrderBy(t => t.FullName);

            var entitiesWithoutHierarchy =
                baseEntities
                    .Where(b => allEntities.Where(e => e.BaseType == b).Count() == 0)
                    .ToList();

            var entitiesWithHierarchy =
                allEntities
                    .Where(e => !entitiesWithoutHierarchy.Contains(e))
                    .ToList();

            mapper.IsEntity((t, declared) => allEntities.Contains(t));
            mapper.IsRootEntity((t, declared) => baseEntities.Contains(t));
            mapper.IsTablePerClassHierarchy((t, declared) => entitiesWithHierarchy.Contains(t));

            mapper.Class<IEntity<TId>>(m =>
            {
                m.Id(e => e.Id, im => im.Generator(Generators.Native));
            });

            return mapper.CompileMappingFor(allEntities);
        }

        public static ISessionFactory CreateSessionFactory<TId>(string connectionString, Type rootEntityType)
        {
            var config = new Configuration();

            config.DataBaseIntegration(db =>
            {
                db.ConnectionString = connectionString;
                db.ConnectionProvider<DriverConnectionProvider>();
                db.Driver<SqlClientDriver>();
                db.Dialect<MsSql2008Dialect>();
            });

            var mappings = NhHelper.CreateMappings<TId>(rootEntityType);
            config.AddDeserializedMapping(mappings, "DomainMapping");

            SchemaMetadataUpdater.QuoteTableAndColumns(config);

            return config.BuildSessionFactory();
        }
    }
}
