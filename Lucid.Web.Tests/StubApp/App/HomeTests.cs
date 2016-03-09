using System.Net;
using FluentAssertions;
using Lucid.Web.Testing.Http;
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
        public void Index_NoRootPath()
        {
            StubApp.Test(http =>
            {
                var response = http.Get("Home");

                response.Text.Should().Contain("root home controller");
            });
        }

        [Test]
        public void Index_VirtualPath()
        {
            StubApp.Test(http =>
            {
                var response = http.Get("~/Home");

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

                response.HttpStatusCode.Should().Be(HttpStatusCode.Redirect);
            });
        }

        [Test]
        public void Post_Values()
        {
            StubApp.Test(http =>
            {
                var response = http.Post("/Home/PostValues", r => r
                    .SetExpectedResponse(HttpStatusCode.OK)
                    .SetFormValue("V1", "test")
                    .SetFormValue("%&V2", "!\"$%^&*()")
                );

                response.Text.Should().Be("V1=test\nV2=!\"$%^&*()");
            });
        }

        [Test]
        public void ReturnCode_Throws()
        {
            try
            {
                StubApp.Test(http =>
                {
                    http.ExpectError();
                    http.Get("/Home/Return500");
                });

                Assert.Fail("Expected UnexpectedStatusCodeException");
            }
            catch (UnexpectedStatusCodeException e)
            {
                e.ExpectedStatusCode.Should().Be(HttpStatusCode.OK);
                e.Response.HttpStatusCode.Should().Be(HttpStatusCode.InternalServerError);
                e.Response.StatusDescription.Should().Be("Internal Server Error");
            }
        }

        [Test]
        public void ReturnCode_AllowsAnyStatusCode()
        {
            StubApp.Test(http =>
            {
                var response = http.Get("/Home/Return500", r => r.SetExpectedResponse(HttpStatusCode.InternalServerError));

                response.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
            });
        }

        [Test]
        public void ReturnCode_AllowsNullStatusCode()
        {
            StubApp.Test(http =>
            {
                var response = http.Get("/Home/Return500", r => r.SetExpectedResponse(null));

                response.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
            });
        }
    }
}
