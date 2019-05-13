using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using FluentAssertions;
using Lucid.Lib.Facade.Exceptions;
using Lucid.Lib.Testing.Execution;
using Microsoft.AspNetCore.Mvc;
using MvcTesting.Html;
using NUnit.Framework;

namespace Lucid.Modules.Security.Tests
{
    public class ControllerTests : ModuleTestSetup.ControllerTest
    {
        [Test]
        public async Task Can_Login()
        {
            ExecutorStub.StubResult(Agreements.Login);

            var client = MvcTestingClient();

            var form = await client
                .GetAsync(Actions.Login())
                .Form<LoginCommand>();

            var response = await form
                .SetText(m => m.UserName, "TestUser")
                .Submit();

            ExecutorStub.VerifySingleExecuted(Agreements.Login);

            client.Cookies.Select(c => c.Name).Should().Contain(ModuleTestSetup.TestStartup.AuthCookieName);

            var redirectUrl = response.ActionResultOf<RedirectResult>().Url;
            redirectUrl.Should().Be("/");
        }

        [Test]
        public async Task Login_RedirectsToOriginalPage()
        {
            ExecutorStub.StubResult(Agreements.Login);

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
