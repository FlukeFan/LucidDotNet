using System;
using FluentAssertions;
using Lucid.Web.Testing.Html;
using Lucid.Web.Testing.Http;
using NUnit.Framework;

namespace Lucid.Web.Tests.Testing.Html
{
    [TestFixture]
    public class FormValueTests
    {
        [Test]
        public void SetValue_WhenReadonly_Throws()
        {
            var formValue = new FormValue("Name", "Value", read_only: true);

            Assert.DoesNotThrow(() => formValue.SetValue("Value"));

            var e = Assert.Throws<Exception>(() => formValue.SetValue("Changed"));

            e.Message.Should().Be("Cannot change readonly input 'Name'");
        }

        [Test]
        public void SetFormValue_WhenSendEmpty_SendsEmptyValue()
        {
            var send = new FormValue("Name", null, sendEmpty: true);

            var request = new Request("");
            send.SetFormValue(request);

            request.FormValues.Count.Should().Be(1);
            request.FormValues.Get("Name").Should().BeNullOrEmpty();
        }

        [Test]
        public void SetFormValue_WhenNotSendEmpty_DoesNotSendEmptyValue()
        {
            var nosend1 = new FormValue("Name", null, sendEmpty: false);
            var nosend2 = new FormValue("Name", "", sendEmpty: false);

            var request = new Request("");
            nosend1.SetFormValue(request);
            nosend2.SetFormValue(request);

            request.FormValues.Count.Should().Be(0);
        }
    }
}
