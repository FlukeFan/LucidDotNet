using System.Threading.Tasks;
using FluentAssertions;
using Lucid.Infrastructure.Lib.Testing.Controller;
using MvcTesting.Html;
using NUnit.Framework;

namespace Lucid.Modules.Account.Tests
{
    public class ControllerTests : ModuleTestSetup.ControllerTest
    {
        [Test]
        public async Task Can_SeeLoginForm()
        {
            var form = await MvcTestingClient()
                .GetAsync(Actions.Index())
                .Form<Login>();

            form.Method.Should().Be("post");
            form.Action.Should().Be(Actions.Index().PrefixLocalhost());

            form.GetText(m => m.UserName).Should().Be("");
        }
    }
}
