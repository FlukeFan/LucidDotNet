using Lucid.Domain.Persistence;
using Lucid.Domain.Testing;
using Lucid.Domain.Tests.Persistence;

namespace Lucid.Domain.Tests.Testing
{
    public class MemoryRepositoryTests : IRepositoryTests
    {
        protected override IRepository<int> New()
        {
            return new MemoryRepository<int>(new ConsistencyInspector());
        }
    }
}
