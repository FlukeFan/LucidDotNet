using Lucid.Domain.Utility;
using Reposify.Testing;

namespace Lucid.Domain.Tests.Utility
{
    public class LucidMemoryRepository : MemoryRepository<int>, ILucidRepository
    {
        public LucidMemoryRepository() : base(new LucidConsistencyInspector()) { }
    }
}
