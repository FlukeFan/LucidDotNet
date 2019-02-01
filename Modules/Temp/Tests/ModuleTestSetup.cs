using Lucid.Infrastructure.Lib.Testing;
using Lucid.Infrastructure.Lib.Testing.Controller;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;
using MvcTesting.AspNetCore;
using NUnit.Framework;

namespace Lucid.Modules.Temp.Tests
{
    [SetUpFixture]
    public class ModuleTestSetup
    {
        public static SetupTestServer<TestStartup> TestServerSetup;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            TestServerSetup = new SetupTestServer<TestStartup>();
        }

        [OneTimeTearDown]
        public void OnTimeTearDown()
        {
            using (TestServerSetup) { }
        }

        [TestFixture]
        public abstract class ControllerTest
        {
            protected SimulatedHttpClient MvcTestingClient() { return TestServerSetup.TestServer.MvcTestingClient(); }
        }

        public class TestStartup : AbstractTestStartup
        {
            public const string AuthCookieName = "TestStartupAuthCookie";

            public override void ConfigureServices(IServiceCollection services)
            {
                services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(o =>
                    {
                        o.LoginPath = Actions.Login();
                        o.Cookie.Name = AuthCookieName;
                    });

                base.ConfigureServices(services);
            }
        }
    }
}
