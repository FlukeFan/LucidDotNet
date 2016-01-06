using FluentAssertions;
using Lucid.Web.Tests.StubApp.Utility;
using NUnit.Framework;

namespace Lucid.Web.Tests.StubApp.App
{
    [TestFixture]
    public class HomeTests : StubAppTest
    {
        [Test]
        public void Index_GetByControllerConvention()
        {
            StubApp.Test(http =>
            {
                var response = http.Get("/");

                response.Should().Contain("root home controller");
            });
        }

        [Test]
        public void Index_GetByActionConvention()
        {
            StubApp.Test(http =>
            {
                var response = http.Get("/Home");

                response.Should().Contain("root home controller");
            });
        }

        [Test]
        public void Index_GetExplicit()
        {
            StubApp.Test(http =>
            {
                var response = http.Get("/Home/Index");

                response.Should().Contain("root home controller");
            });
        }

        [Test]
        public void Post()
        {
            StubApp.Test(http =>
            {
                var response = http.Post("/Home/Post");

                response.Should().Be("posted");
            });
        }
    }
}
