using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Lucid.Lib.Testing.Controller;
using NUnit.Framework;

namespace Lucid.Infrastructure.Host.Web.Tests.Home
{
    [TestFixture]
    public class ControllerTests : WebTests.Controller
    {
        [Test]
        public async Task CanDisplayHomePage()
        {
            var response = await MvcTestingClient().GetAsync("/");

            response.HttpStatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public async Task WhenNotLoggedIn_DisplaysLogin()
        {
            var client = MvcTestingClient();

            StubUserFilter.StubUser = null;

            var response = await client.GetAsync("/");

            response.Doc.Find("#logInOut").InnerHtml.Should().Be("Log In");
        }

        [Test]
        public async Task WhenLoggedIn_DisplaysLogOut()
        {
            var response = await MvcTestingClient().GetAsync("/");

            response.Doc.Find("#logInOut").InnerHtml.Should().Be("Log Out UnitTest");
        }
    }
}
