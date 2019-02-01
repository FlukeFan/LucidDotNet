using System.IO;
using Lucid.Infrastructure.Lib.Domain.SqlServer;
using Lucid.Infrastructure.Lib.Testing;
using Lucid.Infrastructure.Lib.Testing.Controller;
using Lucid.Infrastructure.Lib.Testing.Execution;
using Lucid.Infrastructure.Lib.Testing.SqlServer;
using MvcTesting.AspNetCore;
using NUnit.Framework;

namespace Lucid.Modules.Account.Tests
{
    [SetUpFixture]
    public class ModuleTestSetup
    {
        public static Schema                        Schema;
        public static SetupTestServer<TestStartup>  TestServerSetup;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            Schema =
                SqlTestUtil.UpdateMigrations<DbMigrations.V001.V000.Rev0_CreateUserTable>(
                    schemaName:             "Account",
                    migrationsSourceFolder: Path.Combine(TestUtil.ProjectPath(), "../Module/DbMigrations"));

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
            protected ExecutorStub          ExecutorStub        => _excecutorStub.Stub;

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
