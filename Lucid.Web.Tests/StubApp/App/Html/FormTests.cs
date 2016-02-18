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
            StubApp.Test(http =>
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
