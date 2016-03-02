using System.Linq;
using FluentAssertions;
using Lucid.Web.Testing.Html;
using Lucid.Web.Testing.Http;
using NUnit.Framework;

namespace Lucid.Web.Tests.Testing.Html
{
    [TestFixture]
    public class ScrapedFormTests
    {
        [Test]
        public void Scrape_IgnoresMissingNames()
        {
            var html = @"
                <form>
                    <input type='text' value='form0' />
                </form>
            ";

            var response = new Response { Text = html };
            var form = response.Form<FormModel>();

            form.FormValues.Count().Should().Be(0);
        }

        [Test]
        public void Scrape_AllowsMissingValues()
        {
            var html = @"
                <form>
                    <input type='text' name='Name' />
                </form>
            ";

            var response = new Response { Text = html };
            var form = response.Form<FormModel>();

            form.GetText(m => m.Name).Should().BeNullOrEmpty();
        }

        [Test]
        public void Get()
        {
            var html = @"
                <form>
                    <input type='text' name='Name' value='form0' />
                </form>
            ";

            var response = new Response { Text = html };
            var form = response.Form<FormModel>();

            var formValue = form.Get("Name");

            formValue.Name.Should().Be("Name");
            formValue.Value.Should().Be("form0");
        }

        [Test]
        public void Post()
        {
            var html = @"
                <form>
                    <input type='text' name='Name' value='form0' />
                    <input type='text' name='Name2' value='form1' />
                </form>
            ";

            var response = new Response { Text = html };

            var post = new Request("/tst", "POST");

            response.Form<FormModel>()
                .SetText(m => m.Name, "tst1")
                .SetText(m => m.Name2, "tst2")
                .SetFormValues(post);

            post.FormValues.Count.Should().Be(2);
            post.FormValues.Get("Name").Should().Be("tst1");
            post.FormValues.Get("Name2").Should().Be("tst2");
        }
    }
}
