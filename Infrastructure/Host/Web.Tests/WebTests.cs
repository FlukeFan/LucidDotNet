using Lucid.Infrastructure.Lib.Testing;
using Lucid.Infrastructure.Lib.Testing.Controller;
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

        public class TestStartup : AbstractTestStartup
        {
        }
    }
}