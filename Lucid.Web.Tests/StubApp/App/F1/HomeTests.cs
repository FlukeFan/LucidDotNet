using FluentAssertions;
using Lucid.Web.Tests.StubApp.Utility;
using NUnit.Framework;

namespace Lucid.Web.Tests.StubApp.App.F1
{
    [TestFixture]
    public class HomeTests : StubAppTest
    {
        [Test]
        public void Index_GetByControllerConvention()
        {
            StubApp.Test(http =>
            {
                string response = http.Get("/f1");

                response.Should().Contain("F1/Home/Index");
            });
        }

        [Test]
        public void Index_GetByActionConvention()
        {
            StubApp.Test(http =>
            {
                string response = http.Get("/f1/Home/");

                response.Should().Contain("F1/Home/Index");
            });
        }

        [Test]
        public void Other_GetExplicit()
        {
            StubApp.Test(http =>
            {
                string response = http.Get("/f1/home/other");

                response.Should().Contain("F1/Home/Other");
            });
        }
    }
}
