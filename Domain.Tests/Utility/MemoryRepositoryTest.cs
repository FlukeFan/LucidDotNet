
namespace Lucid.Domain.Tests.Utility
{
    public class MemoryRepositoryTest : IRepositoryTest
    {
        protected override Domain.Utility.IRepository New()
        {
            return new MemoryRepository();
        }
    }
}
