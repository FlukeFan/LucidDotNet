using Lucid.Domain.Utility;

namespace Lucid.Domain.Tests.Utility
{
    public class MemoryRepositoryTests : IRepositoryTests
    {
        protected override Domain.Utility.IRepository New()
        {
            var repository = new MemoryRepository(LucidPersistenceValidator.BeforeSave);
            Registry.Repository = repository;
            return repository;
        }
    }
}
