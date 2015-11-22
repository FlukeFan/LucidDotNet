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
            WebApp = AspNetTestHost.For(@"..\..\..\Demo.Web");
        }

        public static void TearDownWebHost()
        {
            using (WebApp) { }
        }
    }
}
