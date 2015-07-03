using Lucid.Database.Tests;
using Lucid.Domain.Tests.Utility;
using Lucid.Domain.Utility;
using Lucid.Infrastructure.NHibernate;

namespace Lucid.Infrastructure.Tests.NHibernate
{
    public class NhRepositoryTests : IRepositoryTests
    {
        static NhRepositoryTests()
        {
            var environment = BuildEnvironment.Load();
            NhRepository.Init(environment.LucidConnection);
        }

        private NhRepository _repository;

        protected override IRepository New()
        {
            _repository = new NhRepository().Open();
            Registry.Repository = _repository;
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
