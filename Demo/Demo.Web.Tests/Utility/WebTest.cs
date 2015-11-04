using System.IO;
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
            WebApp = new AspNetTestHost(Path.GetFullPath(@"..\..\..\Demo.Web"));
        }

        public static void TearDownWebHost()
        {
            using (WebApp) { }
        }

        [SetUp]
        public virtual void SetUp()
        {
        }
    }
}
