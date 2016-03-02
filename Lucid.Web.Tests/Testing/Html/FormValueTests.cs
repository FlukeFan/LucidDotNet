using System;
using FluentAssertions;
using Lucid.Web.Testing.Html;
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
    }
}
