using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Lucid.Infrastructure.Lib.Testing.Controller;
using Microsoft.AspNetCore.Mvc;
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

        [Test]
        public async Task Can_Login()
        {
            ExecutorStub.StubResult<Login>(new UserBuilder().Value()); // TODO: remove need for this - create return object by default in stub

            var client = MvcTestingClient();

            var form = await client
                .GetAsync(Actions.Index())
                .Form<Login>();

            var response = await form
                .SetText(m => m.UserName, "User1")
                .Submit();

            ExecutorStub.SingleExecuted<Login>().Should().BeEquivalentTo(new Login { UserName = "User1" });

            client.Cookies.Select(c => c.Name).Should().Contain(ModuleTestSetup.TestStartup.AuthCookieName);

            var redirectUrl = response.LastResult.As<RedirectResult>().Url;
            redirectUrl.Should().Be(Actions.LoginSuccess());

            await client.GetAsync(redirectUrl);
        }
    }
}
