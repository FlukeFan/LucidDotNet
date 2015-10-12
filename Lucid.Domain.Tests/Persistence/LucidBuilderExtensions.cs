using Lucid.Domain.Persistence;
using Lucid.Domain.Testing;

namespace Lucid.Domain.Tests.Persistence
{
    public static class LucidBuildExtensions
    {
        public static T Save<T>(this Builder<T> builder, IRepository<int> repository) where T : IEntity<int>
        {
            var entity = builder.Value();
            return repository.Save(entity);
        }
    }
}
