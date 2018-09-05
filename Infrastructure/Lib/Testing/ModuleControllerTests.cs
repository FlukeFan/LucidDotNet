using MvcTesting.AspNetCore;
using NUnit.Framework;

namespace Lucid.Infrastructure.Lib.Testing
{
    [TestFixture]
    public abstract class ModuleControllerTests<TStartup> where TStartup : class
    {
        public ModuleControllerTests(string moduleRootPath)
        {
            TestRegistry.SetupTestServer<TStartup>(moduleRootPath);
        }

        public SimulatedHttpClient MvcTestingClient()
        {
            return TestRegistry.TestServer.MvcTestingClient();
        }
    }
}
