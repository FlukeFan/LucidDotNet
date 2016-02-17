using System;
using FluentAssertions;
using Lucid.Web.Testing.Http;
using NUnit.Framework;

namespace Lucid.Web.Tests.Testing.Http
{
    [TestFixture]
    public class DocumentWrapperTests
    {
        private static DocumentWrapper NewDoc(string html)
        {
            return new Response { Text = html }.Doc;
        }

        [Test]
        public void FindSingleElement()
        {
            var doc = NewDoc("<div><span/><div>");

            doc.FindSingleElement("span").Should().NotBeNull();
        }

        [Test]
        public void FindSingleElement_ThrowsWhenNotFound()
        {
            var doc = NewDoc("<div><span/><div>");

            var e = Assert.Throws<Exception>(() => doc.FindSingleElement("h1"));
            e.Message.Should().Be("Could not find any element matching selector 'h1'");
        }

        [Test]
        public void FindSingleElement_ThrowsWhenMultiple()
        {
            var doc = NewDoc("<div><span/><span/><div>");

            var e = Assert.Throws<Exception>(() => doc.FindSingleElement("span"));
            e.Message.Should().Be("Expected 1 element, but found 2 elements matching selector 'span'");
        }
    }
}
