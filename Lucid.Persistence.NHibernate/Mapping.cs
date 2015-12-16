using System;
using System.Linq;
using Lucid.Domain.Persistence;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Mapping.ByCode;

namespace Lucid.Persistence.NHibernate
{
    public static class Mapping
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
    }
}
