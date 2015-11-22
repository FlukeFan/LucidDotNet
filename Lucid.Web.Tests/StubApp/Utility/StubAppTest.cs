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
            StubApp = AspNetTestHost.For(@"..\..\..\Lucid.Web.StubApp");
        }

        public static void TearDownWebHost()
        {
            using (StubApp) { }
        }
    }
}
