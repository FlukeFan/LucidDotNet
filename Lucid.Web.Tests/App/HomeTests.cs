using System.Web.Mvc;
using FluentAssertions;
using Lucid.Domain.Contract.Account.Commands;
using Lucid.Domain.Contract.Account.Responses;
using Lucid.Web.App.Home;
using Lucid.Web.Tests.Utility;
using Lucid.Web.Utility;
using MvcTesting.Html;
using NUnit.Framework;

namespace Lucid.Web.Tests.App
{
    public class HomeTests : WebTest
    {
        [Test]
        public void Index_RendersView()
        {
            WebAppTest(client =>
            {
                var response = client.Get("/");

                response.Doc.Find("#homeTest").TextContent.Should().Be("Hello world");
            });
        }

        [Test]
        public void Login_GET_RendersForm()
        {
            WebAppTest(client =>
            {
                var get = client.Get(Actions.Login());
                var form = get.Form<Login>();

                form.GetText(m => m.Email).Should().Be("");
            });
        }

        [Test]
        public void Login_POST_ExecutesCommand()
        {
            WebAppTest(client =>
            {
                ExecutorStub.SetupCommand(new Login(), new UserDto
                {
                    UserId = 123,
                    Email = "test name",
                });

                var authenticationSet = false;

                CookieAuthentication.Authenticate = (r, user) =>
                {
                    var lucidUser = user as LucidUser;
                    lucidUser.Id.Should().Be(123);
                    lucidUser.Name.Should().Be("test name");
                    authenticationSet = true;
                };

                var response = client.Get(Actions.Login()).Form<Login>()
                    .SetText(m => m.Email, "user1")
                    .Submit(client);

                ExecutorStub.Executed<Login>(0).ShouldBeEquivalentTo(new Login
                {
                    Email = "user1",
                });

                authenticationSet.Should().BeTrue("authentication should have been set");

                response.ActionResultOf<RedirectResult>().Url.Should().Be(Actions.Index());
            });
        }
    }
}
