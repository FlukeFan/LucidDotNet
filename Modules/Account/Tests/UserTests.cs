using NHibernate.Cfg;
using NHibernate.Connection;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Mapping.ByCode;
using NUnit.Framework;
using Reposify.NHibernate;
using Reposify.Testing;

namespace Lucid.Modules.Account.Tests
{
    [TestFixture]
    public class UserTests
    {
        public class UserBuilder : Builder<User>
        {
            public UserBuilder()
            {
                With(u => u.Name, "TestUser");
                With(u => u.LastLoggedIn, new System.DateTime(2008, 07, 06));
            }
        }

        [Test]
        public void CheckSaveLoad()
        {
            var mapper = new ConventionModelMapper();
            var mappings = NhHelper.CreateConventionalMappings<Registry.Entity>(mapper);
            var config = NhHelper.CreateConfig(mappings, cfg =>
            {
                cfg.DataBaseIntegration(db =>
                {
                    db.ConnectionString = AccountTests.Schema.ConnectionString;
                    db.ConnectionProvider<DriverConnectionProvider>();
                    db.Driver<SqlClientDriver>();
                    db.Dialect<MsSql2012Dialect>();
                    db.KeywordsAutoImport = Hbm2DDLKeyWords.AutoQuote;
                });
            });

            var sf = config.BuildSessionFactory();

            using (var session = sf.OpenSession())
            using (var repository = new NhRepository(session))
            using (var tx = repository.BeginTransaction())
            {
                var user = new UserBuilder()
                    .Value();

                new CheckSaveLoad<User>(user, repository).Check();
            }
        }
    }
}
