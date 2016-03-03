using System;
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
        public void IgnoresMissingNames()
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
        public void AllowsMissingValues()
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
        public void TwoInputsWithSameName()
        {
            var html = @"
                <form>
                    <input type='text' name='Name' value='value1' />
                    <input type='text' name='Name' value='value2' />
                </form>
            ";

            var response = new Response { Text = html };
            var form = response.Form<FormModel>();

            var formValues = form.Get("Name");
            formValues.Length.Should().Be(2);
            formValues[0].Value.Should().Be("value1");
            formValues[1].Value.Should().Be("value2");
        }

        [Test]
        public void Get()
        {
            var html = @"
                <form>
                    <input type='text' name='Name' value='form0' />
                    <input type='text' name='Name_readonly' readonly />
                    <input type='text' name='Name_disabled' disabled />
                </form>
            ";

            var response = new Response { Text = html };
            var form = response.Form<FormModel>();

            var formValue = form.GetSingle("Name");

            formValue.Name.Should().Be("Name");
            formValue.Value.Should().Be("form0");
            formValue.SendEmpty.Should().BeTrue();

            form.GetSingle("Name_readonly").Readonly.Should().BeTrue("should be readonly");
            form.GetSingle("Name_disabled").Disabled.Should().BeTrue("should be disabled");
        }

        [Test]
        public void Checkbox_DoesNotSendEmpty()
        {
            var html = @"
                <form>
                    <input type='checkbox' name='Name' />
                </form>
            ";

            var response = new Response { Text = html };
            var form = response.Form<FormModel>();

            form.GetSingle("Name").SendEmpty.Should().BeFalse();
        }

        [Test]
        public void Get_NoMatchReturnsEmptyList()
        {
            var form = new ScrapedForm<FormModel>();

            var formValues = form.Get("DoesNotExist");

            formValues.Length.Should().Be(0);
        }

        [Test]
        public void GetSingle_NoMatch_ThrowsError()
        {
            var form = new ScrapedForm<FormModel>();

            var e = Assert.Throws<Exception>(() => form.GetSingle("DoesNotExist"));

            e.Message.Should().Be("Could not find entry 'DoesNotExist' in form values");
        }

        [Test]
        public void GetSingle_MultipleMatch_ThrowsError()
        {
            var html = @"
                <form>
                    <input type='text' name='Name' value='value1' />
                    <input type='text' name='Name' value='value2' />
                </form>
            ";

            var response = new Response { Text = html };
            var form = response.Form<FormModel>();

            var e = Assert.Throws<Exception>(() => form.GetSingle("Name"));

            e.Message.Should().Be("Found multiple form values for 'Name'");
        }

        [Test]
        public void Post()
        {
            var html = @"
                <form>
                    <input type='text' name='Name' value='form0' />
                    <input type='text' name='Name2' value='form1' />
                    <input type='text' name='Name3' disabled />
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
