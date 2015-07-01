
using NUnit.Framework;
namespace Lucid.Domain.Tests.Utility
{
    public class MemoryRepositoryTest : IRepositoryTest
    {
        protected override Domain.Utility.IRepository New()
        {
            Assert.Ignore("wip");
            return new MemoryRepository();
        }
    }
}
