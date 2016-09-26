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
            WebAppTest(client =>
            {
                var response = client.Get("/");

                response.Doc.Find("#homeTest").TextContent.Should().Be("Hello world");
            });
        }
    }
}
