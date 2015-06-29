using System.Data;
using System.Data.SqlClient;
using NUnit.Framework;

namespace Lucid.Database.Tests.Migrations
{
    [TestFixture]
    public class TestMigrations
    {
        private const string DropDb = @"
            IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'Lucid')
            BEGIN
                ALTER DATABASE Lucid
                    SET SINGLE_USER
                    WITH ROLLBACK IMMEDIATE

                DROP DATABASE Lucid
            END";

        private const string CreateDb = "Create Database Lucid";

        [Test]
        [Ignore("WIP - not ready for DB yet")]
        public void Migrations_Run_UpgradesDatabase()
        {
            DropAndCreateBlankDb();

            Assert.Fail("got to here");
        }

        private void ExecuteNonQuery(IDbConnection connection, string sql)
        {
            var cmd = connection.CreateCommand();
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
        }

        private void DropAndCreateBlankDb()
        {
            var environment = BuildEnvironment.Load();

            using (var masterDb = new SqlConnection(environment.MasterConnection))
            {
                masterDb.Open();
                ExecuteNonQuery(masterDb, DropDb);
                ExecuteNonQuery(masterDb, CreateDb);
            }
        }
    }
}
