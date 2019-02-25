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
            using (var db = new ModuleTestSetup.DbTxTest())
            {
                var entity = new BlueprintBuilder()
                    .Value();

                new CheckSaveLoad<Blueprint>(entity, db.NhRepository).Check();
            }
        }
    }
}
