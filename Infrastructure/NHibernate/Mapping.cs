using System.Linq;
using Lucid.Domain.Utility;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Mapping.ByCode;

namespace Lucid.Infrastructure.NHibernate
{
    public static class Mapping
    {
        public static HbmMapping CreateMappings()
        {
            var mapper = new ConventionModelMapper();

            var baseEntities =
                typeof(Entity).Assembly.GetTypes()
                    .Where(t => t.BaseType == typeof(Entity))
                    .OrderBy(t => t.FullName);

            var allEntities =
                typeof(Entity).Assembly.GetTypes()
                    .Where(t => typeof(Entity).IsAssignableFrom(t) && t != typeof(Entity))
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

            mapper.Class<Entity>(m =>
            {
                m.Id(e => e.Id, im => im.Generator(Generators.Native));
            });

            return mapper.CompileMappingFor(allEntities);
        }
    }
}
