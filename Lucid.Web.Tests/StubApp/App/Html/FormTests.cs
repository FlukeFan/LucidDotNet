using AngleSharp.Parser.Html;
using FluentAssertions;
using Lucid.Web.Tests.StubApp.Utility;
using NUnit.Framework;

namespace Lucid.Web.Tests.StubApp.App.Html
{
    [TestFixture]
    public class FormTests : StubAppTest
    {
        [Test]
        public void Form_Render()
        {
            StubApp.Test(http =>
            {
                var response = http.Get("/html/form_render");

                var parser = new HtmlParser();
                var dom = parser.Parse(response.Text);

                var element = dom.QuerySelector("form");

                element.Should().NotBeNull();
                var method = element.Attributes.GetNamedItem("method");
                method.Value.Should().Be("post");
            });
        }
    }
}
