using Lucid.Database.Tests;
using Lucid.Domain.Persistence;
using Lucid.Domain.Tests.Persistence;

namespace Lucid.Infrastructure.Persistence.NHibernate.Tests
{
    public class NhRepositoryTests : IRepositoryTests
    {
        static NhRepositoryTests()
        {
            var environment = BuildEnvironment.Load();
            NhRepository<int>.Init(environment.LucidConnection, typeof(LucidEntity));
        }

        private NhRepository<int> _repository;

        protected override IRepository<int> New()
        {
            _repository = new NhRepository<int>().Open();
            return _repository;
        }

        public override void TearDown()
        {
            using (_repository) { }
            _repository = null;

            base.TearDown();
        }
    }
}
