using System;
using NUnit.Framework;
using Reposify.Testing;

namespace Lucid.Modules.AppFactory.Design.Tests
{
    [TestFixture]
    public class BlueprintTests
    {
        [Test]
        public void CheckSaveLoad()
        {
            using (var db = new ModuleTestSetup.SetupDbTx())
            {
                var entity = new BlueprintBuilder()
                    .Value();

                new CheckSaveLoad<Blueprint>(entity, db.NhRepository).Check();
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

        private void VerifyConstraints(Action<Builder<Blueprint>> verifySave)
        {
            Func<Builder<Blueprint>> validBuilder = () => new BlueprintBuilder();

            Assert.That(() => verifySave(validBuilder()), Throws.Nothing);

            Assert.That(() => verifySave(validBuilder().With(u => u.Name, null)), Throws.Exception, $"should not allow null Name");
        }
    }
}
