using System.Net;
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

                response.Text.Should().Contain("root home controller");
            });
        }

        [Test]
        public void Index_GetByActionConvention()
        {
            StubApp.Test(http =>
            {
                var response = http.Get("/Home");

                response.Text.Should().Contain("root home controller");
            });
        }

        [Test]
        public void Index_GetExplicit()
        {
            StubApp.Test(http =>
            {
                var response = http.Get("/Home/Index");

                response.Text.Should().Contain("root home controller");
            });
        }

        [Test]
        public void Post()
        {
            StubApp.Test(http =>
            {
                var response = http.Post("/Home/Post");

                response.Text.Should().Be("posted");
            });
        }

        [Test]
        public void ReturnCode_Throws()
        {
            StubApp.Test(http =>
            {
                var response = http.Get("/Home/Return500");

                response.HttpStatusCode.Should().Be(HttpStatusCode.InternalServerError);
                response.StatusDescription.Should().Be("Internal Server Error");
            });
        }
    }
}
