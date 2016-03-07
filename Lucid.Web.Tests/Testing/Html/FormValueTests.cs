using System;
using System.Collections.Generic;
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
        public void SetFormValue_WhenSend_SendsValues()
        {
            var send_value = new FormValue("send_value", "value", send: true);
            var send_null = new FormValue("send_null", null, send: true);
            var send_empty = new FormValue("send_empty", "", send: true);

            var request = new Request("");
            send_value.SetFormValue(request);
            send_null.SetFormValue(request);
            send_empty.SetFormValue(request);

            request.FormValues.Count.Should().Be(3);
            request.FormValues.Get("send_value").Should().Be("value");
            request.FormValues.Get("send_null").Should().BeNull();
            request.FormValues.Get("send_empty").Should().BeEmpty();
        }

        [Test]
        public void SetFormValue_WhenNotSend_DoesNotSend()
        {
            var nosend = new FormValue("Name", "value", send: false);

            var request = new Request("");
            nosend.SetFormValue(request);

            request.FormValues.Count.Should().Be(0);
        }

        [Test]
        public void Construct_WhenTextsIsNotSameLengthAsConfinedValues_Throws()
        {
            Assert.Throws<Exception>(() => new FormValue("name", confinedValues: new List<string> { "a" }, texts: new List<string> { "b", "c" }));
        }

        [Test]
        public void SetValue_WhenNotInConfinedValues_Throws()
        {
            var formValue = new FormValue("Name", "a", confinedValues: new List<string> { "a", "b" });

            Assert.DoesNotThrow(() => formValue.SetValue("b"));

            formValue.Value.Should().Be("b");

            var e = Assert.Throws<Exception>(() => formValue.SetValue("c"));

            e.Message.Should().Be("Value 'c' cannot be set.  Must be one of 'a, b'");
        }

        [Test]
        public void ForceValue()
        {
            var formValue = new FormValue("Name", "a", confinedValues: new List<string> { "a", "b" });

            formValue.ForceValue("c");

            formValue.Value.Should().Be("c");
        }

        [Test]
        public void SelectText()
        {
            var formValue = new FormValue("Name", "a", confinedValues: new List<string> { "a", "b" }, texts: new List<string> { "a value", "b value" });

            formValue.SelectText("b value");

            formValue.Value.Should().Be("b");
        }

        [Test]
        public void SelectText_WhenTextNotFound_Throws()
        {
            var formValue = new FormValue("Name", "a", confinedValues: new List<string> { "a", "b" }, texts: new List<string> { "a value", "b value" });

            var e = Assert.Throws<Exception>(() => formValue.SelectText("c value"));

            e.Message.Should().Be("Could not find 'c value' in 'a value, b value'");
        }
    }
}
