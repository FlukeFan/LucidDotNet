using Demo.Web.Tests.Utility;
using FluentAssertions;
using NUnit.Framework;

namespace Demo.Web.Tests.App.Html
{
    [TestFixture]
    public class FormTests : WebTest
    {
        [Test]
        public void Form_Render()
        {
            WebAppTest(http =>
            {
                var response = http.Get("/html/form_render?p1=123");

                response.Doc.Find("form").Where(f =>
                {
                    f.Attribute("method").Should().Be("post");
                    f.Attribute("action").Should().Be("/html/form_render?p1=123");
                });
            });
        }

        [Test]
        public void Form_RenderInput()
        {
            WebAppTest(http =>
            {
                var response = http.Get("/html/form_renderinput");

                response.Doc.Find("form input").Where(f =>
                {
                    f.Attribute("name").Should().Be("Name");
                    f.Attribute("value").Should().Be("NameValue");
                });
            });
        }
    }
}
