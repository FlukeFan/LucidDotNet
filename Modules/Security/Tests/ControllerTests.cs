using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using FluentAssertions;
using Lucid.Infrastructure.Lib.Facade.Exceptions;
using Lucid.Infrastructure.Lib.Testing.Controller;
using Microsoft.AspNetCore.Mvc;
using MvcTesting.Html;
using NUnit.Framework;

namespace Lucid.Modules.Security.Tests
{
    public class ControllerTests : ModuleTestSetup.ControllerTest
    {
        [Test]
        public async Task Can_SeeLoginForm()
        {
            var form = await MvcTestingClient()
                .GetAsync(Actions.Login())
                .Form<LoginCommand>();

            form.Method.Should().Be("post");
            form.Action.Should().Be(Actions.Login().PrefixLocalhost());

            form.GetText(m => m.UserName).Should().Be("");
        }

        [Test]
        public async Task Can_Login()
        {
            ExecutorStub.StubResult<LoginCommand>(new UserBuilder().Value());

            var client = MvcTestingClient();

            var form = await client
                .GetAsync(Actions.Login())
                .Form<LoginCommand>();

            var response = await form
                .SetText(m => m.UserName, "User1")
                .Submit();

            ExecutorStub.SingleExecuted<LoginCommand>().Should().BeEquivalentTo(new LoginCommand { UserName = "User1" });

            client.Cookies.Select(c => c.Name).Should().Contain(ModuleTestSetup.TestStartup.AuthCookieName);

            var redirectUrl = response.ActionResultOf<RedirectResult>().Url;
            redirectUrl.Should().Be("/");
        }

        [Test]
        public async Task Login_RedirectsToOriginalPage()
        {
            ExecutorStub.StubResult<LoginCommand>(new UserBuilder().Value());

            var action = Actions.Login() + $"/?returnUrl={HttpUtility.UrlEncode("http://someUrl")}";

            var form = await MvcTestingClient()
                .GetAsync(action)
                .Form<LoginCommand>();

            var response = await form
                .SetText(m => m.UserName, "User1")
                .Submit();

            response.ActionResultOf<RedirectResult>().Url.Should().Be("http://someUrl");
        }

        [Test]
        public async Task WhenError_RedisplaysPage()
        {
            ExecutorStub.StubResult<LoginCommand>(l => throw new FacadeException("simulated error"));

            var form = await MvcTestingClient().GetAsync(Actions.Login()).Form<LoginCommand>();
            await form.Submit(r => r.SetExpectedResponse(HttpStatusCode.OK));
        }

        [Test]
        public async Task CanLogOut()
        {
            var client = MvcTestingClient();

            var response = await client
                .GetAsync(Actions.LogOut(), r => r.SetExpectedResponse(HttpStatusCode.Redirect));

            client.Cookies.Select(c => c.Name).Should().Contain(ModuleTestSetup.TestStartup.AuthCookieName);

            var redirectUrl = response.ActionResultOf<RedirectResult>().Url;
            redirectUrl.Should().Be("/");
        }
    }
}
