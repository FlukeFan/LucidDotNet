using Lucid.Infrastructure.Lib.Testing;
using Lucid.Infrastructure.Lib.Testing.Execution;
using Lucid.Infrastructure.Lib.Testing.SqlServer;
using NUnit.Framework;

namespace Lucid.Modules.Account.Tests
{
    [SetUpFixture]
    public class AccountTests
    {
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            // need to figure out if anything has changed, and if it hasn't, skip remainder of the DB setup

            var schema = SqlTestUtil.DropAll("Account");
            SqlTestUtil.UpdateMigrations<DbMigrations.V001.V000.Rev0_CreateUserTable>(schema);
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
