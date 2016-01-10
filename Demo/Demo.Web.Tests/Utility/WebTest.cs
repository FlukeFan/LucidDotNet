using System;
using System.Diagnostics;
using Lucid.Domain.Testing;
using Lucid.Web.Testing.Hosting;
using Lucid.Web.Testing.Http;
using NUnit.Framework;

namespace Demo.Web.Tests.Utility
{
    [TestFixture]
    public abstract class WebTest
    {
        private static AspNetTestHost _webApp;

        protected ExecutorStub ExecutorStub {[DebuggerStepThrough] get { return WebTestRegistry.ExecutorStub; } }

        public static void SetUpWebHost()
        {
            _webApp = AspNetTestHost.For(@"..\..\..\Demo.Web", typeof(TestEnvironmentProxy));
        }

        public static void TearDownWebHost()
        {
            using (_webApp) { }
        }

        protected void WebAppTest(Action<SimulatedHttpClient> test)
        {
            _webApp.Test(http =>
            {
                WebTestRegistry.SetupExecutorStub();
                test(http);
            });
        }

        private class TestEnvironmentProxy : AppDomainProxy
        {
            public TestEnvironmentProxy()
            {
                DemoApplication.Startup = new FakeStartup();
            }
        }
    }
}
