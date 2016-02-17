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

                response.Doc.FindSingle("form").Attribute("method").Should().Be("post");
            });
        }
    }
}
