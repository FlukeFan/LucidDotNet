using System.Data.SqlClient;
using NUnit.Framework;

namespace Lucid.Database.Tests.Migrations
{
    [TestFixture]
    public class TestMigrations
    {
        [Test]
        public void Migrations_CanUpgradeTablesContaingData()
        {
            DropAndCreateBlankDb();

            Assert.Fail("got to here");
        }

        private void DropAndCreateBlankDb()
        {
            var environment = BuildEnvironment.Load();
            var masterDb = new SqlConnection(environment.MasterConnection);
            masterDb.Open();
        }
    }
}
