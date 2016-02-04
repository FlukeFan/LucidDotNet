using Lucid.Persistence.Testing;

namespace Lucid.Persistence.Tests
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
