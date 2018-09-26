using System.IO;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Lucid.Infrastructure.Lib.Testing;
using Microsoft.AspNetCore.Mvc;
using MvcTesting.AspNetCore;
using NUnit.Framework;

namespace Lucid.Infrastructure.Host.Mvc.Tests
{
    [TestFixture]
    public class ViewTests
    {
        [Test][Ignore("WIP - need to wire up IActionResult from MvcTesting")]
        public async Task CanSeeModuleViews()
        {
            var client = TestRegistry
                .SetupTestServer<Startup>(Path.Combine(TestUtil.ProjectPath(), "../Mvc"))
                .MvcTestingClient();

            var response = await client.GetAsync("/");

            response.HttpStatusCode.Should().Be(HttpStatusCode.OK);
            response.LastResult.Should().BeOfType<ViewResult>();
        }
    }
}
