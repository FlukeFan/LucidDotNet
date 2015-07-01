
namespace Lucid.Domain.Tests.Utility
{
    public class MemoryRepositoryTests : IRepositoryTests
    {
        protected override Domain.Utility.IRepository New()
        {
            return new MemoryRepository();
        }
    }
}
