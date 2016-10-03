using System.Data;
using System.Data.SqlClient;
using FluentAssertions;
using NUnit.Framework;

namespace Lucid.Database.Tests.Migrations
{
    [TestFixture]
    public class MigrationsTests
    {
        private DatabaseSettings        _databaseSettings       = new DatabaseSettings();
        private DatabaseTestsSettings   _databaseTestSettings   = new DatabaseTestsSettings();

        private const string DropDb = @"
            IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'Lucid')
            BEGIN
                ALTER DATABASE Lucid
                    SET SINGLE_USER
                    WITH ROLLBACK IMMEDIATE

                DROP DATABASE Lucid
            END";

        private const string CreateDb = "CREATE DATABASE Lucid";

        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            Settings.Init("..\\..\\..\\Lucid.Web\\settings.config",
                _databaseSettings,
                _databaseTestSettings);
        }

        [Test]
        [Explicit("Run when cleaning the build")]
        public void DropDatabase()
        {
            using (var masterDb = new SqlConnection(_databaseTestSettings.MasterConnection))
            {
                masterDb.Open();
                ExecuteNonQuery(masterDb, DropDb);
            }
        }

        [Test]
        public void Migrations_Run_UpgradesDatabase()
        {
            DropAndCreateBlankDb();

            LucidMigrationRunner.Run(_databaseSettings.LucidConnection);

            // verify we can run a second time (i.e., scripts don't get run twice where they shouldn't)
            LucidMigrationRunner.Run(_databaseSettings.LucidConnection);

            using (var db = new SqlConnection(_databaseSettings.LucidConnection))
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
            using (var masterDb = new SqlConnection(_databaseTestSettings.MasterConnection))
            {
                masterDb.Open();
                ExecuteNonQuery(masterDb, DropDb);
                ExecuteNonQuery(masterDb, CreateDb);
            }
        }
    }
}
