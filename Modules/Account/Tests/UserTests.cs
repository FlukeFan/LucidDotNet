using System;
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

        [Test]
        public void DbConstraints()
        {
            VerifyConstraints(e =>
            {
                using (var db = new ModuleTestSetup.DbTxTest())
                    e.Save(db.NhRepository);
            });
        }

        private void VerifyConstraints(Action<Builder<User>> verifySave)
        {
            Func<Builder<User>> validUserBuilder = () => new UserBuilder();

            Assert.That(() => verifySave(validUserBuilder()), Throws.Nothing);

            Assert.That(() => verifySave(validUserBuilder().With(u => u.Name, null)), Throws.Exception, $"should not allow null Name");
            Assert.That(() => verifySave(validUserBuilder().With(u => u.LastLoggedIn, DateTime.MinValue)), Throws.Exception, $"should not allow date-value of {DateTime.MinValue}");
        }
    }
}
