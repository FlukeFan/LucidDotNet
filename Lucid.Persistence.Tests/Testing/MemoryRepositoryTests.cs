using Lucid.Persistence.Testing;

namespace Lucid.Persistence.Tests.Testing
{
    public class MemoryRepositoryTests : IRepositoryTests
    {
        protected override IRepository<int> New()
        {
            return new MemoryRepository<int>(new ConsistencyInspector());
        }
    }
}
