using FluentAssertions;
using Lucid.Web.Tests.StubApp.Utility;
using NUnit.Framework;

namespace Lucid.Web.Tests.StubApp.App.F1
{
    [TestFixture]
    public class F1Tests : StubAppTest
    {
        [Test]
        public void Index_GetByConvention()
        {
            StubApp.Test(http =>
            {
                string response = http.Get("/f1");

                response.Should().Contain("F1/Home/Index");
            });
        }

        [Test]
        public void Index_GetExplicit()
        {
            StubApp.Test(http =>
            {
                string response = http.Get("/f1/Home");

                response.Should().Contain("F1/Home/Index");
            });
        }

        [Test]
        public void Other_GetByConvention()
        {
            StubApp.Test(http =>
            {
                string response = http.Get("/f1/other");

                response.Should().Contain("F1/Home/Other");
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
