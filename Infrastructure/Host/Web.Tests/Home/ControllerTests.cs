using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
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
    }
}
