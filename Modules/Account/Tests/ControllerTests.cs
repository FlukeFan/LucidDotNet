using System.Threading.Tasks;
using FluentAssertions;
using MvcTesting.Html;
using NUnit.Framework;

namespace Lucid.Modules.Account.Tests
{
    public class ControllerTests : AccountTests.Controller
    {
        [Test]
        public async Task Can_SeeLoginForm()
        {
            var form = await MvcTestingClient()
                .GetAsync(Actions.Index())
                .Form<Login>();

            form.Method.Should().Be("post");
            form.Action.Should().Be(AddHost(Actions.Index()));

            form.GetText(m => m.UserName).Should().Be("");
        }
    }
}
