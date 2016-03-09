using System;
using FluentAssertions;
using Lucid.Web.Testing.Html;
using Lucid.Web.Testing.Http;
using NUnit.Framework;

namespace Lucid.Web.Tests.Testing.Html
{
    [TestFixture]
    public class TypedFormTests
    {
        [Test]
        public void Get_NoMatchReturnsEmptyList()
        {
            var form = new TypedForm<FormModel>();

            var formValues = form.Get("DoesNotExist");

            formValues.Length.Should().Be(0);
        }

        [Test]
        public void GetSingle_NoMatch_ThrowsError()
        {
            var form = new TypedForm<FormModel>();

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
        public void Method_DefaultsToGet()
        {
            var html = @"<form />";
            var form = new Response { Text = html }.Form<FormModel>();

            form.Method.Should().Be("get");
        }

        [Test]
        public void Method_Post()
        {
            var html = @"<form method='post' />";
            var form = new Response { Text = html }.Form<FormModel>();

            form.Method.Should().Be("post");
        }

        [Test]
        public void Method_WhenNotGetOrPost_DefaultsToGet()
        {
            var html = @"<form method='put' />";
            var form = new Response { Text = html }.Form<FormModel>();

            form.Method.Should().Be("get");
        }

        [Test]
        public void Action()
        {
            var html = @"<form action='/test' />";
            var form = new Response { Text = html }.Form<FormModel>();

            form.Action.Should().Be("/test");
        }

        [Test]
        public void Action_DefaultsToEmpty()
        {
            var html = @"<form />";
            var form = new Response { Text = html }.Form<FormModel>();

            form.Action.Should().Be("");
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
