using System;
using FluentAssertions;
using Lucid.Web.Testing.Http;
using NUnit.Framework;

namespace Lucid.Web.Tests.Testing.Http
{
    [TestFixture]
    public class ElementWrapperTests
    {
        private static ElementWrapper NewElem(string html, string cssSelector)
        {
            return new Response { Text = html }.Doc.Find(cssSelector);
        }

        [Test]
        public void Attribute()
        {
            var elem = NewElem("<div id='div1'></div>", "div");

            var value = elem.Attribute("id");

            value.Should().NotBeNull();
            value.Should().Be("div1");
        }

        [Test]
        public void Attribute_ThrowsWhenNotFound()
        {
            var elem = NewElem("<div id='div1'></div>", "div");

            var e = Assert.Throws<Exception>(() => elem.Attribute("idx"));

            e.Message.Should().Be("Could not find attribute 'idx' on element <div id=\"div1\">");
        }

        [Test]
        public void Where()
        {
            var elem = NewElem("<div id='div1'></div>", "div");

            elem.Where(e =>
            {
                e.Attribute("id").Should().Be("div1");
                Assert.Throws<Exception>(() => e.Attribute("idx"));
            });
        }

        [Test]
        public void InnerText()
        {
            var elem = NewElem("<div>some text</div>", "div");

            elem.TextContent.Should().Be("some text");
        }
    }
}
