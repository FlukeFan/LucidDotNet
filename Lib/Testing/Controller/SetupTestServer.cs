using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using MvcTesting.AspNetCore;

namespace Lucid.Lib.Testing.Controller
{
    public class SetupTestServer<TStartup> : IDisposable where TStartup : class
    {
        private Lazy<TestServer> _testServer;

        public SetupTestServer()
        {
            _testServer = new Lazy<TestServer>(() =>
                new WebHostBuilder()
                    .UseConfiguration(TestUtil.GetConfig())
                    .UseEnvironment("Testing")
                    .UseStartup<TStartup>()
                    .MvcTestingTestServer());
        }

        public TestServer TestServer => _testServer.Value;

        public void Dispose()
        {
            if (_testServer.IsValueCreated)
                using (_testServer.Value) { }
        }
    }
}
