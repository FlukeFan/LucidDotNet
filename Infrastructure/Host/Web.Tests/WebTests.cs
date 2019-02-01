using System.Linq;
using Lucid.Infrastructure.Lib.Domain.SqlServer;
using Lucid.Infrastructure.Lib.Testing.Controller;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MvcTesting.AspNetCore;
using NUnit.Framework;

namespace Lucid.Infrastructure.Host.Web.Tests
{
    [SetUpFixture]
    public class WebTests
    {
        private static SetupTestServer<TestStartup> _testServerSetup;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _testServerSetup = new SetupTestServer<TestStartup>();
        }

        [OneTimeTearDown]
        public void OnTimeTearDown()
        {
            using (_testServerSetup) { }
        }

        public abstract class Controller
        {
            protected SimulatedHttpClient MvcTestingClient() { return _testServerSetup.TestServer.MvcTestingClient(); }
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

            protected override void InitSqlServer(IConfigurationSection config, params Schema[] schemas)
            {
                // no SQL init
                foreach (var schema in schemas)
                    schema.ConnectionString = "Server=not_used;User ID=not_used;Password=not_used";
            }
        }
    }
}