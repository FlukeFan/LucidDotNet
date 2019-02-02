using NUnit.Framework;
using Reposify.NHibernate;
using Reposify.Testing;

namespace Lucid.Modules.Account.Tests
{
    [TestFixture]
    public class UserTests
    {
        [Test]
        public void CheckSaveLoad()
        {
            using (var session = ModuleTestSetup.SessionFactory.Value.OpenSession())
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
