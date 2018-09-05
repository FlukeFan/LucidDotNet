using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using MvcTesting.AspNetCore;

namespace Lucid.Infrastructure.Lib.Testing
{
    public static class TestRegistry
    {
        private static TestServer _testServer;

        public static TestServer TestServer { get { return _testServer; } }

        public static void SetupTestServer<TStartup>(string modulePath) where TStartup : class
        {
            _testServer = new WebHostBuilder()
                .UseEnvironment("Development")
                .UseContentRoot(modulePath)
                .UseStartup<TStartup>()
                .MvcTestingTestServer();
        }
    }
}
