using System.Data;
using System.Data.SqlClient;
using FluentAssertions;
using NUnit.Framework;

namespace Lucid.Database.Tests.Migrations
{
    [TestFixture]
    public class MigrationsTests
    {
        private static BuildEnvironment _environment = BuildEnvironment.Load();

        private const string DropDb = @"
            IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'Lucid')
            BEGIN
                ALTER DATABASE Lucid
                    SET SINGLE_USER
                    WITH ROLLBACK IMMEDIATE

                DROP DATABASE Lucid
            END";

        private const string CreateDb = "CREATE DATABASE Lucid";

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

            LucidMigrationRunner.Run(_environment.LucidConnection);

            // verify we can run a second time (i.e., scripts don't get run twice where they shouldn't)
            LucidMigrationRunner.Run(_environment.LucidConnection);

            using (var db = new SqlConnection(_environment.LucidConnection))
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
