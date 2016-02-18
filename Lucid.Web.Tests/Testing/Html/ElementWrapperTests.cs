using System;
using FluentAssertions;
using Lucid.Web.Testing.Html;
using Lucid.Web.Testing.Http;
using NUnit.Framework;

namespace Lucid.Web.Tests.Testing.Html
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
        public void HasAttribute()
        {
            var elem = NewElem("<div id='div1'></div>", "div");

            elem.HasAttribute("id").Should().BeTrue();
            elem.HasAttribute("idx").Should().BeFalse();
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
        public void WrapperProperties()
        {
            var elem = NewElem("<div id='d1' class='c1 c2'>some <span>text</span></div>", "#d1");

            elem.ClassName.Should().Be("c1 c2");
            elem.Id.Should().Be("d1");
            elem.InnerHtml.Should().Be("some <span>text</span>");
            elem.OuterHtml.Should().StartWith("<div").And.EndWith("</div>");
            elem.TagName.Should().Be("DIV");
            elem.TextContent.Should().Be("some text");
        }
    }
}
