using Demo.Domain.Utility;
using Lucid.Domain.Persistence;

namespace Lucid.Domain.Tests.Utility
{
    public class MemoryRepositoryTests : IRepositoryTests
    {
        protected override IRepository<int> New()
        {
            var repository = new DemoMemoryRepository(LucidPersistenceValidator.BeforeSave);
            Registry.Repository = repository;
            return repository;
        }
    }
}
