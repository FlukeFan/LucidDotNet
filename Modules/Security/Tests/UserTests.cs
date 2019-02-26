using System;
using NUnit.Framework;
using Reposify.Testing;

namespace Lucid.Modules.Security.Tests
{
    [TestFixture]
    public class UserTests
    {
        [Test]
        public void CheckSaveLoad()
        {
            using (var db = new ModuleTestSetup.SetupDbTx())
            {
                var entity = new UserBuilder()
                    .Value();

                new CheckSaveLoad<User>(entity, db.NhRepository).Check();
            }
        }

        [Test]
        public void DbConstraints()
        {
            VerifyConstraints(e =>
            {
                using (var db = new ModuleTestSetup.SetupDbTx())
                    e.Save(db.NhRepository);
            });
        }

        [Test]
        public void MemoryConstraints()
        {
            VerifyConstraints(e =>
            {
                using (var mem = new ModuleTestSetup.SetupMemoryLogic())
                    e.Save(mem.MemoryRepository);
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
