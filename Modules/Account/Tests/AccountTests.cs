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
            SqlTestUtil.UpdateMigrations<DbMigrations.V001.V000.Script000>(schema);
        }
    }
}
