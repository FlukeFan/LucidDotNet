using System.Data;
using System.Data.SqlClient;
using FluentAssertions;
using NUnit.Framework;

namespace Demo.Database.Tests.Migrations
{
    [TestFixture]
    public class MigrationsTests
    {
        private static BuildEnvironment _environment = BuildEnvironment.Load();

        private const string DropDb = @"
            IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'Demo')
            BEGIN
                ALTER DATABASE Demo
                    SET SINGLE_USER
                    WITH ROLLBACK IMMEDIATE

                DROP DATABASE Demo
            END";

        private const string CreateDb = "CREATE DATABASE Demo";

        [Test]
        [Explicit("Run when cleaning the build")]
        public void DropDatabase()
        {
            using (var masterDb = new SqlConnection(_environment.MasterConnection))
            {
                masterDb.Open();
                ExecuteNonQuery(masterDb, DropDb);
            }
        }

        [Test]
        public void Migrations_Run_UpgradesDatabase()
        {
            DropAndCreateBlankDb();

            DemoMigrationRunner.Run(_environment.DemoConnection);

            // verify we can run a second time (i.e., scripts don't get run twice where they shouldn't)
            DemoMigrationRunner.Run(_environment.DemoConnection);

            using (var db = new SqlConnection(_environment.DemoConnection))
            {
                db.Open();

                var cmd = db.CreateCommand();
                cmd.CommandText = "select count(*) from [User]";
                var rows = (int)cmd.ExecuteScalar();

                rows.Should().Be(0);
            }
        }

        private void ExecuteNonQuery(IDbConnection connection, string sql)
        {
            var cmd = connection.CreateCommand();
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
        }

        private void DropAndCreateBlankDb()
        {
            using (var masterDb = new SqlConnection(_environment.MasterConnection))
            {
                masterDb.Open();
                ExecuteNonQuery(masterDb, DropDb);
                ExecuteNonQuery(masterDb, CreateDb);
            }
        }
    }
}
