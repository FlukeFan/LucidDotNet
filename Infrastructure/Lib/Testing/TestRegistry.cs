using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using MvcTesting.AspNetCore;

namespace Lucid.Infrastructure.Lib.Testing
{
    public static class TestRegistry
    {
        public static Type          CurrentStartup  { get; private set; }
        public static TestServer    TestServer      { get; private set; }

        public static TestServer SetupTestServer<TStartup>() where TStartup : class
        {
            var requestedStartup = typeof(TStartup);

            if (CurrentStartup == requestedStartup)
                return TestServer; // already setup

            CurrentStartup = requestedStartup;

            TestServer = new WebHostBuilder()
                .UseConfiguration(new ConfigurationBuilder().AddEnvironmentVariables().Build())
                .UseEnvironment("Testing")
                .UseStartup<TStartup>()
                .MvcTestingTestServer();

            return TestServer;
        }
    }
}
