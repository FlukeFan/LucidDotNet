using Demo.Web.Tests.Utility;
using FluentAssertions;
using NUnit.Framework;

namespace Demo.Web.Tests.App
{
    public class HomeTests : WebTest
    {
        [Test]
        public void Index_RendersView()
        {
            WebApp.Test(client =>
            {
                var response = client.Get("/");

                response.Should().Contain("<title>Home page</title>");
                response.Should().Contain("Hello world");
            });
        }
    }
}
