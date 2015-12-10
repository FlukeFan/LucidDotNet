using Demo.Web.Utility;
using Lucid.Web.Testing.Hosting;
using NUnit.Framework;

namespace Demo.Web.Tests.Utility
{
    [TestFixture]
    public abstract class WebTest
    {
        protected static AspNetTestHost WebApp { get; private set; }

        public static void SetUpWebHost()
        {
            WebApp = AspNetTestHost.For(@"..\..\..\Demo.Web", typeof(TestEnvironmentProxy));
        }

        public static void TearDownWebHost()
        {
            using (WebApp) { }
        }

        private class TestEnvironmentProxy : AppDomainProxy
        {
            public TestEnvironmentProxy()
            {
                DemoApplication.Startup = new FakeStartup();
            }
        }

        private class FakeStartup : Startup
        {
            public override void InitRepository()
            {
                // no repository to setup during the web tests
            }
        }
    }
}
