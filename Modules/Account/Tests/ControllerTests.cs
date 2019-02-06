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
            ExecutorStub.StubResult<Login>(new UserBuilder().Value());

            var client = MvcTestingClient();

            var form = await client
                .GetAsync(Actions.Index())
                .Form<Login>();

            var response = await form
                .SetText(m => m.UserName, "User1")
                .Submit();

            ExecutorStub.SingleExecuted<Login>().Should().BeEquivalentTo(new Login { UserName = "User1" });

            client.Cookies.Select(c => c.Name).Should().Contain(ModuleTestSetup.TestStartup.AuthCookieName);

            var redirectUrl = response.ActionResultOf<RedirectResult>().Url;
            redirectUrl.Should().Be(Actions.LoginSuccess());

            await client.GetAsync(redirectUrl);
        }

        [Test]
        public async Task Login_RedirectsToOriginalPage()
        {
            ExecutorStub.StubResult<Login>(new UserBuilder().Value());

            var action = Actions.Index() + $"/?returnUrl={HttpUtility.UrlEncode("http://someUrl")}";

            var form = await MvcTestingClient()
                .GetAsync(action)
                .Form<Login>();

            var response = await form
                .SetText(m => m.UserName, "User1")
                .Submit();

            response.ActionResultOf<RedirectResult>().Url.Should().Be("http://someUrl");
        }

        [Test]
        public async Task WhenError_RedisplaysPage()
        {
            ExecutorStub.StubResult<Login>(l => throw new FacadeException("simulated error"));

            var form = await MvcTestingClient().GetAsync(Actions.Index()).Form<Login>();
            await form.Submit(r => r.SetExpectedResponse(HttpStatusCode.OK));
        }
    }
}
