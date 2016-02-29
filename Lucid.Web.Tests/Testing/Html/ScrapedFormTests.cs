using System;
using FluentAssertions;
using Lucid.Web.Testing.Http;
using NUnit.Framework;

namespace Lucid.Web.Tests.Testing.Html
{
    [TestFixture]
    public class ScrapedFormTests
    {
        [Test]
        public void Scrape_ScrapeFormByIndex()
        {
            var html = @"
                <form><input type='text' name='Name' value='form0' /></form>
                <form><input type='text' name='Name' value='form1' /></form>
            ";

            var response = new Response { Text = html };
            var form = response.Form<Model>(1);

            form.GetText(m => m.Name).Should().Be("form1");
        }

        [Test]
        public void Scrape_SingleForm()
        {
            var html = @"
                <form><input type='text' name='Name' value='form0' /></form>
            ";

            var response = new Response { Text = html };
            var form = response.Form<Model>();

            form.GetText(m => m.Name).Should().Be("form0");
        }

        [Test]
        public void Scrape_MultipleFormsWithoutIndex_Throws()
        {
            var html = @"
                <form id='1'><input type='text' name='Name' value='form0' /></form>
                <form id='2'><input type='text' name='Name' value='form1' /></form>
            ";

            var response = new Response { Text = html };
            var e = Assert.Throws<Exception>(() => response.Form<Model>());

            e.Message.Should().Be("Multiple form elements found in document: <form id=\"1\">\n<form id=\"2\">");
        }

        [Test]
        public void Scrape_NoFormsFound_Throws()
        {
            var html = @"
                <input type='text' name='Name' value='form0' />
                <input type='text' name='Name' value='form1' />
            ";

            var response = new Response { Text = html };
            var e = Assert.Throws<Exception>(() => response.Form<Model>());

            e.Message.Should().Be("CSS selector 'form' did not match any elements in the document");
        }

        [Test]
        public void Scrape_IndexTooLarge_Throws()
        {
            var html = @"
                <form index='0'><input type='text' name='Name' value='form0' /></form>
            ";

            var response = new Response { Text = html };
            var e = Assert.Throws<Exception>(() => response.Form<Model>(1));

            e.Message.Should().Be("Index '1' is too large for collection with '1' forms: <form index=\"0\">");
        }

        [Test]
        public void Scrape_SpecifySelector()
        {
            var html = @"
                <form id='1'><input type='text' name='Name' value='form0' /></form>
                <form id='2'><input type='text' name='Name' value='form1' /></form>
            ";

            var response = new Response { Text = html };
            var form = response.Form<Model>("form[id=2]");

            form.GetText(m => m.Name).Should().Be("form1");
        }

        private class Model
        {
            public string Name { get; set; }
        }
    }
}
