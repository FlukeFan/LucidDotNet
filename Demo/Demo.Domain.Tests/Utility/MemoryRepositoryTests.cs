using Demo.Domain.Utility;
using Lucid.Domain.Persistence;

namespace Lucid.Domain.Tests.Utility
{
    public class MemoryRepositoryTests : IRepositoryTests
    {
        protected override IRepository<int> New()
        {
            var validator = new DemoConsistencyInspector();
            var repository = new DemoMemoryRepository(e => validator.BeforeSave(e));
            Registry.Repository = repository;
            return repository;
        }
    }
}
