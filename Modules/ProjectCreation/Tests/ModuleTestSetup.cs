using Lucid.Lib.Testing;
using Lucid.Lib.Testing.Controller;
using Lucid.Lib.Testing.Execution;
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

            protected SimulatedHttpClient   MvcTestingClient()  { return TestServerSetup.TestServer.MvcTestingClient(); }
            protected ExecutorStubAsync     ExecutorStub        => _excecutorStub.Stub;

            [SetUp]
            public void SetUp()
            {
                _excecutorStub = new SetupExecutorStub(Registry.ExecutorAsync, e => Registry.ExecutorAsync = e);
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
