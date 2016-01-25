using System.Linq;
using Demo.Database.Migrations;
using Demo.Database.Migrations.Y2016.M01;
using FluentAssertions;
using FluentMigrator;
using NUnit.Framework;

namespace Demo.Database.Tests.Migrations
{
    [TestFixture]
    public class ConstraintsTests
    {
        [Test]
        public void Verify_MigrationNumber_Matches_ScriptName()
        {
            // verify that all migration are numbered according to the namespace and name they use
            // e.g., a script V35 in namespace Y2016.M09 should be script number 20160935

            var migrationTypes = typeof(V01).Assembly.GetTypes()
                .Where(t => !t.IsAbstract)
                .Where(t => typeof(DemoMigration).IsAssignableFrom(t));

            foreach (var migrationType in migrationTypes)
            {
                var migrationAttribute = migrationType.GetCustomAttributes(typeof(MigrationAttribute), false).Cast<MigrationAttribute>().SingleOrDefault();

                if (migrationAttribute == null)
                    Assert.Fail("Could not find MigrationAttribute on {0}", migrationType);

                var actualVersion = migrationAttribute.Version;
                var expectedVersion = 0L;
                var nameParts = migrationType.FullName.Split('.');

                foreach (var namePart in nameParts)
                {
                    var numeralsAfterLetters = new string(namePart.SkipWhile(c => char.IsLetter(c)).ToArray());

                    if (string.IsNullOrEmpty(numeralsAfterLetters))
                        continue;

                    for (var i = 0; i < numeralsAfterLetters.Length; i++)
                        expectedVersion *= 10;

                    expectedVersion += long.Parse(numeralsAfterLetters);
                }

                actualVersion.Should().Be(expectedVersion, "version on {0} should match namespace and name", migrationType);
            }
        }

        [Test]
        public void Verify_AllDeployedScripts_HaveHash()
        {
            Assert.Ignore("Not implemented yet");
        }
    }
}
