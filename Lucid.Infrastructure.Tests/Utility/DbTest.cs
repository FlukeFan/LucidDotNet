using Lucid.Database;
using Lucid.Domain.Tests.Utility;
using Lucid.Domain.Utility;
using Lucid.Infrastructure.NHibernate;
using NUnit.Framework;

namespace Lucid.Infrastructure.Tests.Utility
{
    [TestFixture]
    public abstract class DbTest
    {
        private static DatabaseSettings _databaseSettings = new DatabaseSettings();

        protected LucidNhRepository Repository { get; set; }

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            Settings.Init("..\\..\\..\\Lucid.Web\\settings.config", _databaseSettings);
        }

        [SetUp]
        public virtual void SetUp()
        {
            LucidNhRepository.Init(_databaseSettings.LucidConnection);
            Repository = new LucidNhRepository();
            Repository.Open();
            DomainRegistry.Repository = Repository;
        }

        [TearDown]
        public virtual void TearDown()
        {
            using (Repository) { }
            DomainRegistry.Repository = null;
        }

        protected void VerifyInvalidConstraint<T>(T entity) where T : LucidEntity
        {
            Assert.That(() => Repository.Save(entity), Throws.Exception, string.Format("Entity {0} did not fail to save in the database", entity));
            Assert.That(() => new LucidMemoryRepository().Save(entity), Throws.Exception, string.Format("Entity {0} correctly failed to save in the database, but did not fail to save in the memory repository", entity));
        }
    }
}
