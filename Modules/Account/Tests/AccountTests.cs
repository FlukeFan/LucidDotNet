using System.IO;
using Lucid.Infrastructure.Lib.Domain.SqlServer;
using Lucid.Infrastructure.Lib.Testing;
using Lucid.Infrastructure.Lib.Testing.Execution;
using Lucid.Infrastructure.Lib.Testing.SqlServer;
using NUnit.Framework;

namespace Lucid.Modules.Account.Tests
{
    [SetUpFixture]
    public class AccountTests
    {
        public static Schema Schema;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            Schema =
                SqlTestUtil.UpdateMigrations<DbMigrations.V001.V000.Rev0_CreateUserTable>(
                    schemaName:             "Account",
                    migrationsSourceFolder: Path.Combine(TestUtil.ProjectPath(), "../Module/DbMigrations"));
        }

        public abstract class Controller : ModuleControllerTests<TestStartup>
        {
            protected ExecutorStub ExecutorStub { get; private set; }

            [SetUp]
            public void SetUp()
            {
                Registry.Executor = ExecutorStub = new ExecutorStub();
            }
        }

        public class TestStartup : AbstractTestStartup
        {
        }
    }
}
