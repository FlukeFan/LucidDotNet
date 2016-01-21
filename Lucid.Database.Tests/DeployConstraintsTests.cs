using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Lucid.Database.Tests
{
    [TestFixture]
    public class DeployConstraintsTests
    {
        [Test]
        public void Verify_VersionIsUpdated_AfterDeployToNuGet()
        {
            var nugetExe = Path.Combine(Environment.CurrentDirectory, "..\\..\\..\\packages\\NuGet.CommandLine.3.3.0\\tools\\NuGet.exe");
            var localCopyFolder = Path.Combine(Environment.CurrentDirectory, "..\\..\\..\\packages_deployed");
            var localCopyVersionFile = Path.Combine(localCopyFolder, "version.txt");

            var deployedVersion = GetDeployedVersion(nugetExe);
            var localCopyVersion = GetLocalCopyVersion(localCopyVersionFile);

            if (deployedVersion == null && localCopyVersionFile == null)
                Assert.Fail("Cannot determine deployed version of Lucid");

            if (deployedVersion != null && localCopyVersion != deployedVersion)
                DownloadLocalCopy(nugetExe, localCopyFolder, localCopyVersionFile, deployedVersion);

            localCopyVersion = GetLocalCopyVersion(localCopyVersionFile);

            Assert.Ignore("Got current version: '" + localCopyVersion + "'");
        }

        private string GetDeployedVersion(string nugetExe)
        {
            var result = Run.Program(nugetExe, "list Lucid.Domain.Testing");

            if (result.ExitCode != 0)
            {
                Console.WriteLine("Output from NuGet.exe:\n" + string.Join("\n", result.Output));
                Console.WriteLine("Errors from NuGet.exe:\n" + string.Join("\n", result.Errors));
                Console.WriteLine("Unable to get version from NuGet online");
                return null;
            }

            var failMessage = "Output from NuGet.exe:\n" + string.Join("\n", result.Output);

            if (result.Output.Count != 1)
                Assert.Fail("Expected output to contain a single line, but got " + failMessage);

            var lines = result.Output[0].Split(' ');

            if (lines.Length != 2 || lines[0] != "Lucid.Domain.Testing")
                Assert.Fail("Expected output to contain a single line, but got " + failMessage);

            return lines[1];
        }

        private string GetLocalCopyVersion(string localCopyVersionFile)
        {
            if (!File.Exists(localCopyVersionFile))
                return null;

            return File.ReadAllText(localCopyVersionFile);
        }

        private void DownloadLocalCopy(string nugetExe, string localCopyFolder, string localCopyVersionFile, string deployedVersion)
        {
            // the internet has a more up-to-date copy, so download that

            if (Directory.Exists(localCopyFolder))
                Directory.Delete(localCopyFolder, true);

            foreach (var nuspecFile in Directory.GetFiles("..\\..\\..\\", "Lucid.*.nuspec", SearchOption.AllDirectories))
            {
                var packageName = Path.GetFileNameWithoutExtension(nuspecFile);

                Run.Program(nugetExe, string.Format("install {0} -o \"{1}\"", packageName, localCopyFolder));
            }

            File.WriteAllText(localCopyVersionFile, deployedVersion);
        }
    }
}
