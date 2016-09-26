using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using FluentAssertions;
using Lucid.Database.Migrations;
using Lucid.Database.Migrations.Y2016.M01;
using NUnit.Framework;

namespace Lucid.Database.Tests.Migrations
{
    [TestFixture]
    public class ConstraintsTests
    {
        [Test]
        public void Verify_MigrationNumber_Matches_ScriptName()
        {
            // verify that all migration are numbered according to the namespace and name they use
            // e.g., a script V35 in namespace Y2016.M09 should be script number 20160935

            var migrationTypes = FindMigrationTypes();

            foreach (var migrationType in migrationTypes)
            {
                var actualVersion = VersionUtil.GetVersion(migrationType);
                var expectedVersion = 0L;
                var nameParts = migrationType.FullName.Replace("Lucid.Database.", "").Split('.');

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
        [Category("DeployBuild")]
        [Explicit("Run explcitly during creation of a deployment build")]
        public void Verify_AllDeployedScripts_HaveHash()
        {
            VerifyHashes(failIfNoHash: true);
        }

        [Test]
        public void Verify_AllDeployedScripts_HaveNotChangedHash()
        {
            VerifyHashes(failIfNoHash: false);
        }

        private void VerifyHashes(bool failIfNoHash)
        {
            var projectDir = FindProjectDir();
            var migrationsSourceDir = Path.Combine(projectDir, "Lucid.Database/Migrations");
            var migrationTypes = FindMigrationTypes();
            var errors = new List<string>();

            foreach (var migrationType in migrationTypes)
            {
                var nspace = migrationType.Namespace;
                var folders = nspace.Replace("Lucid.Database.Migrations.", "").Replace(".", "\\");
                var filename = migrationType.Name + ".cs";
                var migrationFile = Path.Combine(Path.Combine(migrationsSourceDir, folders), filename);

                if (!File.Exists(migrationFile))
                {
                    errors.Add(string.Format("Could not find source file {0} for migration {1}", migrationFile, migrationType));
                    continue;
                }

                using (var md5 = MD5.Create())
                {
                    var content = File.ReadAllText(migrationFile);
                    var bytes = Encoding.ASCII.GetBytes(content);
                    var hashBytes = md5.ComputeHash(bytes);

                    var hashBuilder = new StringBuilder();
                    foreach (var b in hashBytes)
                        hashBuilder.Append(b.ToString("X2"));

                    var hash = hashBuilder.ToString();
                    var hashes = DeployedMigrations.Hashes;

                    if (!hashes.ContainsKey(migrationType))
                    {
                        if (failIfNoHash)
                            errors.Add(string.Format("Migration {0} needs an entry in DeployedMigrations with hash {1}", migrationType, hash));

                        continue;
                    }

                    if (hashes[migrationType] != hash)
                    {
                        errors.Add(string.Format("Migration {0} has an entry with hash {1}, but expected entry with hash {2} - examine the script history to ensure changes are not lost", migrationType, hashes[migrationType], hash));
                        continue;
                    }
                }
            }

            if (errors.Count > 0)
                Assert.Fail("Could not deploy database scripts:\n\n{0}\n", string.Join("\n\n", errors));
        }

        private IEnumerable<Type> FindMigrationTypes()
        {
            return typeof(V01).Assembly.GetTypes()
                .Where(t => !t.IsAbstract)
                .Where(t => typeof(LucidMigration).IsAssignableFrom(t));
        }

        private string FindProjectDir()
        {
            var searchFile = "Lucid.sln";
            var envVars = Environment.GetEnvironmentVariables();
            var searchDir = (string)envVars["project_src_dir"];
            var error = "";

            if (searchDir == null)
            {
                error += "no ProjectDir found in Environment variables and ";
                searchDir = GetType().Assembly.CodeBase;
                searchDir = searchDir.Replace("file:///", "");
            }

            searchDir = Path.GetDirectoryName(searchDir); // strip any filename
            var projectDir = searchDir;
            var parent = Directory.GetParent(projectDir);

            while (parent != null && parent.FullName != projectDir && !File.Exists(Path.Combine(projectDir, searchFile)))
            {
                projectDir = parent.FullName;
                parent = Directory.GetParent(projectDir);
            }

            if (!File.Exists(Path.Combine(projectDir, searchFile)))
                Assert.Fail(error + "could not find {0} in parent of folder {1}", searchFile, searchDir);

            return projectDir;
        }
    }
}
