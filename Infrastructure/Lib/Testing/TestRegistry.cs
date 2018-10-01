using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using MvcTesting.AspNetCore;

namespace Lucid.Infrastructure.Lib.Testing
{
    public static class TestRegistry
    {
        public static TestServer TestServer { get; private set; }

        public static TestServer SetupTestServer<TStartup>() where TStartup : class
        {
            TestServer = new WebHostBuilder()
                .UseEnvironment("Testing")
                .UseStartup<TStartup>()
                .MvcTestingTestServer();

            return TestServer;
        }
    }
}
