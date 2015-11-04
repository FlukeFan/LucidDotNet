using Demo.Web.Tests.Utility;
using FluentAssertions;
using NUnit.Framework;

namespace Demo.Web.Tests.Controllers
{
    public class HomeControllerTests : WebTest
    {
        [Test]
        public void Index_RendersView()
        {
            WebApp.Test(client =>
            {
                var response = client.Get("/");
                response.Should().Be("Hello world");
            });
        }
    }
}
