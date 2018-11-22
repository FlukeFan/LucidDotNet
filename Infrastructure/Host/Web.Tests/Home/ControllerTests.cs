using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Lucid.Infrastructure.Lib.Testing;
using MvcTesting.AspNetCore;
using NUnit.Framework;

namespace Lucid.Infrastructure.Host.Web.Tests.Home
{
    [TestFixture]
    public class ControllerTests
    {
        [Test]
        public async Task CanDisplayHomePage()
        {
            var client = TestRegistry
                .SetupTestServer<ViewTests.TestStartup>()
                .MvcTestingClient();

            var response = await client.GetAsync("/");

            response.HttpStatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
