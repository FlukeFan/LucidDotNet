using NUnit.Framework;
using Reposify.Testing;

namespace Lucid.Modules.Account.Tests
{
    [TestFixture]
    public class UserTests
    {
        [Test]
        public void CheckSaveLoad()
        {
            using (var db = new ModuleTestSetup.DbTxTest())
            {
                var user = new UserBuilder()
                    .Value();

                new CheckSaveLoad<User>(user, db.NhRepository).Check();
            }
        }
    }
}
