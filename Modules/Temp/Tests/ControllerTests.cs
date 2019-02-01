using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Lucid.Modules.Temp.Tests
{
    public class ControllerTests : ModuleTestSetup.ControllerTest
    {
        [Test]
        public async Task Index()
        {
            var response = await MvcTestingClient()
                .GetAsync(Actions.Index());

            response.HttpStatusCode.Should().Be(HttpStatusCode.OK);
            response.Text.Should().Contain("precompiled");
        }

        [Test]
        public async Task Login()
        {
            var client = MvcTestingClient();

            var response = await client
                .GetAsync(Actions.Login());

            client.Cookies.Select(c => c.Name).Should().Contain(ModuleTestSetup.TestStartup.AuthCookieName);
        }
    }
}
