using FluentAssertions;
using Lucid.Web.Testing.Html;
using Lucid.Web.Testing.Http;
using NUnit.Framework;

namespace Lucid.Web.Tests.Testing.Html
{
    [TestFixture]
    public class TypedFormExtensionsTests
    {
        [Test]
        public void GetText()
        {
            var html = @"
                <form>
                    <input type='text' name='Name' value='form0' />
                </form>
            ";

            var form = new Response { Text = html }.Form<FormModel>();

            form.GetText(m => m.Name).Should().Be("form0");
        }
    }
}
