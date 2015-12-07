using Lucid.Web.Testing.Hosting;
using NUnit.Framework;

namespace Lucid.Web.Tests.StubApp.Utility
{
    [TestFixture]
    public abstract class StubAppTest
    {
        protected static AspNetTestHost StubApp { get; private set; }

        public static void SetUpWebHost()
        {
            StubApp = AspNetTestHost.For(@"..\..\..\Lucid.Web.StubApp", typeof(TestHostStartup));
        }

        public static void TearDownWebHost()
        {
            using (StubApp) { }
        }

        private class TestHostStartup : AppDomainProxy
        {
            public TestHostStartup()
            {
                Lucid.Web.StubApp.App.Home.HomeController.RootHomeControllerResponseText = "root home controller";
            }
        }
    }
}
