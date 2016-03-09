using System;
using System.Linq;
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
            formValue.Send.Should().BeTrue();

            form.GetSingle("Name_readonly").Readonly.Should().BeTrue("should be readonly");
            form.GetSingle("Name_disabled").Disabled.Should().BeTrue("should be disabled");
        }

        [Test]
        public void Checkbox()
        {
            var html = @"
                <form>
                    <input type='checkbox' name='cb_noValue_notChecked' />
                    <input type='checkbox' name='cb_noValue_checked' checked />
                    <input type='checkbox' name='cb_value_notChecked' value='123' />
                    <input type='checkbox' name='cb_value_checked' value='234' checked />
                </form>
            ";

            var response = new Response { Text = html };
            var form = response.Form<FormModel>();

            form.GetSingle("cb_noValue_notChecked").Value.Should().Be("on");
            form.GetSingle("cb_noValue_notChecked").Send.Should().BeFalse();

            form.GetSingle("cb_noValue_checked").Value.Should().Be("on");
            form.GetSingle("cb_noValue_checked").Send.Should().BeTrue();

            form.GetSingle("cb_value_notChecked").Value.Should().Be("123");
            form.GetSingle("cb_value_notChecked").Send.Should().BeFalse();

            form.GetSingle("cb_value_checked").Value.Should().Be("234");
            form.GetSingle("cb_value_checked").Send.Should().BeTrue();
        }

        [Test]
        public void Radio()
        {
            var html = @"
                <form>
                    <input type='radio' name='r_noValue_checked' checked />
                    <input type='radio' name='r_value_notChecked' value='123' />
                </form>
            ";

            var response = new Response { Text = html };
            var form = response.Form<FormModel>();

            form.GetSingle("r_noValue_checked").Value.Should().Be("on");
            form.GetSingle("r_noValue_checked").Send.Should().BeTrue();

            form.GetSingle("r_value_notChecked").Value.Should().Be("123");
            form.GetSingle("r_value_notChecked").Send.Should().BeFalse();
        }

        [Test]
        public void Select()
        {
            var html = @"
                <form>
                    <select name='Selector'>
                        <option>notset</option>
                        <option value=''>empty</option>
                        <option value='v1' selected>value 1</option>
                        <option value='v2'>value 2</option>
                    </select>
                </form>
            ";

            var response = new Response { Text = html };
            var form = response.Form<FormModel>();
            var select = form.GetSingle("Selector");

            select.Value.Should().Be("v1");
            select.ConfinedValues.Should().BeEquivalentTo("notset", "", "v1", "v2");
            select.Texts.Should().BeEquivalentTo("notset", "empty", "value 1", "value 2");
        }

        [Test]
        public void TextArea()
        {
            var html = @"
                <form>
                    <textarea name='ta_disabled' disabled></textarea>
                    <textarea name='ta_readonly' readonly></textarea>
                    <textarea name='ta'>text area</textarea>
                </form>
            ";

            var response = new Response { Text = html };
            var form = response.Form<FormModel>();

            form.GetSingle("ta_disabled").Disabled.Should().BeTrue();
            form.GetSingle("ta_readonly").Readonly.Should().BeTrue();
            form.GetSingle("ta").Value.Should().Be("text area");
        }

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
            var response = new Response { Text = html };
            var form = response.Form<FormModel>();

            form.Method.Should().Be("get");
        }

        [Test]
        public void Method_Post()
        {
            var html = @"<form method='post' />";
            var response = new Response { Text = html };
            var form = response.Form<FormModel>();

            form.Method.Should().Be("post");
        }

        [Test]
        public void Method_WhenNotGetOrPost_DefaultsToGet()
        {
            var html = @"<form method='put' />";
            var response = new Response { Text = html };
            var form = response.Form<FormModel>();

            form.Method.Should().Be("get");
        }

        [Test]
        public void Action()
        {
            var html = @"<form action='/test' />";
            var response = new Response { Text = html };
            var form = response.Form<FormModel>();

            form.Action.Should().Be("/test");
        }

        [Test]
        public void Action_DefaultsToEmpty()
        {
            var html = @"<form />";
            var response = new Response { Text = html };
            var form = response.Form<FormModel>();

            form.Action.Should().Be("");
        }

        [Test]
        public void ScrapeSubmit()
        {
            var html = @"
                <form>
                    <input type='submit' />
                    <input type='submit' name='NoValue' />
                    <input type='submit' name='SubmitName' value='Submit Value' />
                </form>
            ";

            var response = new Response { Text = html };
            var form = response.Form<FormModel>();

            form.SubmitValues.Count().Should().Be(3);
            {
                var sb = form.SubmitValues.ElementAt(0);
                sb.Name.Should().Be("");
            }
            {
                var sb = form.SubmitValues.ElementAt(1);
                sb.Name.Should().Be("NoValue");
                sb.Value.Should().Be("Submit");
            }
            {
                var sb = form.SubmitValues.ElementAt(2);
                sb.Name.Should().Be("SubmitName");
                sb.Value.Should().Be("Submit Value");
            }
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
