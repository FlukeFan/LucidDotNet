using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Lucid.Infrastructure.Lib.Testing;
using Microsoft.AspNetCore.Mvc;
using MvcTesting.AspNetCore;
using NUnit.Framework;

namespace Lucid.Infrastructure.Host.Web.Tests
{
    [TestFixture]
    public class ViewTests
    {
        [Test]
        public async Task CanSeeModuleViews()
        {
            var client = TestRegistry
                .SetupTestServer<TestStartup>()
                .MvcTestingClient();

            var response = await client.GetAsync("/");

            response.HttpStatusCode.Should().Be(HttpStatusCode.OK);
            response.LastResult.Should().BeOfType<ViewResult>();
        }

        [Test]
        public async Task CanSeeModuleViewsWithSameViewName()
        {
            var client = TestRegistry
                .SetupTestServer<TestStartup>()
                .MvcTestingClient();

            var response = await client.GetAsync("/temp");

            response.HttpStatusCode.Should().Be(HttpStatusCode.OK);
            response.LastResult.Should().BeOfType<ViewResult>();
        }

        public class TestStartup : Startup
        {
            protected override void ConfigureMvcOptions(MvcOptions mvcOptions)
            {
                base.ConfigureMvcOptions(mvcOptions);
                mvcOptions.Filters.Add<CaptureResultFilter>();
            }
        }
    }
}
