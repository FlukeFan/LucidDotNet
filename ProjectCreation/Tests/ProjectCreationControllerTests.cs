using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using MvcTesting.AspNetCore;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class ProjectCreationControllerTests
    {
        private TestServer _testServer;

        [OneTimeSetUp]
        public void SetUpFixture()
        {
            _testServer = new WebHostBuilder()
                .UseEnvironment("Development")
                .UseContentRoot(@"..\..\..\..\Module")
                .UseStartup<TestStartup>()
                .MvcTestingTestServer();
        }

        [Test]
        public async Task Index_DisplaysForm()
        {
            var response = await _testServer.MvcTestingClient()
                .GetAsync("/ProjectCreation");

            response.Text.Should().StartWith("Hello from deployed");
        }
    }

    public class TestStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMvc(routes =>
            {
            });
        }
    }
}
