using Demo.Web.Tests.Utility;
using FluentAssertions;
using NUnit.Framework;

namespace Demo.Web.Tests.App.Html
{
    [TestFixture]
    public class ComponentTests : WebTest
    {
        [Test]
        public void Component_Render()
        {
            WebAppTest(http =>
            {
                var response = http.Get("/html/component_render");

                response.Text.Should().Contain("<li>item 1 - &lt;div&gt; encoded &lt;/div&gt;");
                response.Text.Should().Contain("<li>item 2 - not encoded</li>");
            });
        }
    }
}
