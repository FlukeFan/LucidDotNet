using NUnit.Framework;

namespace Lucid.Modules.AppFactory.Manufacturing.Tests
{
    public class EmptyTest : ModuleTestSetup.LogicTest
    {
        [Test]
        public void InitialEmptyTest()
        {
            using (new ModuleTestSetup.SetupDbTx())
            {
                Assert.Pass("Added empty module");
            }
        }
    }
}
