
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Lucid.Infrastructure.Lib.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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

            var response = await client.GetAsync(Modules.ProjectCreation.Actions.Index());

            response.HttpStatusCode.Should().Be(HttpStatusCode.OK);
            response.LastResult.Should().BeOfType<ViewResult>();
        }

        [Test]
        public async Task CanSeeModuleViewsWithSameViewName()
        {
            var client = TestRegistry
                .SetupTestServer<TestStartup>()
                .MvcTestingClient();

            var response = await client.GetAsync(Modules.Temp.Actions.Index());

            response.HttpStatusCode.Should().Be(HttpStatusCode.OK);
            response.LastResult.Should().BeOfType<ViewResult>();
        }

        public class TestStartup : Startup
        {
            public TestStartup(IHostingEnvironment env, ILogger<Startup> log) : base(env, log) { }

            protected override void ConfigureMvcOptions(MvcOptions mvcOptions)
            {
                base.ConfigureMvcOptions(mvcOptions);
                mvcOptions.Filters.Add<CaptureResultFilter>();

                // remove authorization filter for testing
                mvcOptions.Filters.Remove(mvcOptions.Filters.Where(f => typeof(AuthorizeFilter).IsAssignableFrom(f.GetType())).Single());
            }

            protected override void InitSqlServer(IConfigurationSection config)
            {
                // no SQL init
            }
        }
    }
}
