using MvcTesting.AspNetCore;
using NUnit.Framework;

namespace Lucid.Infrastructure.Lib.Testing
{
    [TestFixture]
    public abstract class ModuleControllerTests<TStartup> where TStartup : class
    {
        public ModuleControllerTests()
        {
            TestRegistry.SetupTestServer<TStartup>();
        }

        public static string ProjectPath()
        {
            return TestUtil.ProjectPath();
        }

        public SimulatedHttpClient MvcTestingClient()
        {
            return TestRegistry.TestServer.MvcTestingClient();
        }

        protected string AddHost(string action)
        {
            return $"http://localhost{action}";
        }
    }
}
