using FluentAssertions;
using Lucid.Web.Tests.StubApp.Utility;
using NUnit.Framework;

namespace Lucid.Web.Tests.StubApp.App.Html
{
    [TestFixture]
    public class ComponentTests : StubAppTest
    {
        [Test]
        public void RenderComponent()
        {
            StubApp.Test(http =>
            {
                var response = http.Get("/html/component_render");

                response.Text.Should().Contain("<li>item 1 - &lt;div&gt; encoded &lt;/div&gt;");
                response.Text.Should().Contain("<li>item 2 - not encoded</li>");
            });
        }
    }
}
