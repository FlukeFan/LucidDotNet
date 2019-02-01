using Lucid.Infrastructure.Lib.Testing;
using Lucid.Infrastructure.Lib.Testing.Controller;
using Lucid.Infrastructure.Lib.Testing.Execution;
using MvcTesting.AspNetCore;
using NUnit.Framework;

namespace Lucid.Modules.ProjectCreation.Tests
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
            private SetupExecutorStub _excecutorStub;

            protected SimulatedHttpClient MvcTestingClient() { return TestServerSetup.TestServer.MvcTestingClient(); }
            protected ExecutorStub ExecutorStub => _excecutorStub.Stub;

            [SetUp]
            public void SetUp()
            {
                _excecutorStub = new SetupExecutorStub(ref Registry.Executor, prev => Registry.Executor = prev);
            }

            [TearDown]
            public void TearDown()
            {
                using (_excecutorStub) { }
            }
        }

        public class TestStartup : AbstractTestStartup
        {
        }
    }
}
