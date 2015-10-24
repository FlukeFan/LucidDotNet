using Demo.Database.Tests;
using Demo.Domain.Tests.Utility;
using Demo.Domain.Utility;
using Demo.Infrastructure.NHibernate;
using NUnit.Framework;

namespace Demo.Infrastructure.Tests.Utility
{
    [TestFixture]
    public abstract class DbTest
    {
        private static BuildEnvironment _buildEnvironment = BuildEnvironment.Load();

        protected DemoNhRepository Repository { get; set; }

        [SetUp]
        public virtual void SetUp()
        {
            DemoNhRepository.Init(_buildEnvironment.DemoConnection, typeof(DemoEntity));
            Repository = new DemoNhRepository();
            Repository.Open();
            Registry.Repository = Repository;
        }

        [TearDown]
        public virtual void TearDown()
        {
            using (Repository) { }
            Registry.Repository = null;
        }

        protected void VerifyInvalidConstraint<T>(T entity) where T : DemoEntity
        {
            Assert.That(() => Repository.Save(entity), Throws.Exception, string.Format("Entity {0} did not fail to save in the database", entity));
            Assert.That(() => new DemoMemoryRepository().Save(entity), Throws.Exception, string.Format("Entity {0} correctly failed to save in the database, but did not fail to save in the memory repository", entity));
        }
    }
}
