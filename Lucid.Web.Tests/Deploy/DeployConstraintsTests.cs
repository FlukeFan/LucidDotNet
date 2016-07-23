using System;
using System.IO;
using NUnit.Framework;

namespace Lucid.Web.Tests.Deploy
{
    [TestFixture]
    public class DeployConstraintsTests
    {
        private const string NugetPackageName   = "Lucid.Web";
        private const string NugetSourceOption  = " -Source https://api.nuget.org/v3/index.json";
        private const string NugetExeLocation   = "packages\\NuGet.CommandLine.3.4.3\\tools\\NuGet.exe";

        [Test]
        public void Verify_VersionIsUpdated_AfterDeployToNuGet()
        {
            var nugetExe = Path.Combine(Environment.CurrentDirectory, "..\\..\\..\\" + NugetExeLocation);
            var localCopyFolder = Path.Combine(Environment.CurrentDirectory, "..\\..\\..\\packages_deployed");
            var localCopyVersionFile = Path.Combine(localCopyFolder, "version.txt");

            var deployedVersion = GetDeployedVersion(nugetExe, NugetSourceOption);
            var localCopyVersion = GetLocalCopyVersion(localCopyVersionFile);

            if (deployedVersion == null && localCopyVersionFile == null)
                Assert.Fail("Cannot determine deployed version of Lucid");

            if (deployedVersion != null && localCopyVersion != deployedVersion)
                DownloadLocalCopy(nugetExe, NugetSourceOption, localCopyFolder, localCopyVersionFile, deployedVersion);

            localCopyVersion = GetLocalCopyVersion(localCopyVersionFile);

            var thisVersion = GetThisVersion();

            if (thisVersion == localCopyVersion)
                Assert.Fail(string.Format("Version {0} is currently deployed. Update the version in Common.targets to a newer version.", thisVersion));
        }

        private string GetDeployedVersion(string nugetExe, string sourceOption)
        {
            var result = Run.Program(nugetExe, "list " + NugetPackageName + sourceOption);

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

            if (lines.Length != 2 || lines[0] != NugetPackageName)
                Assert.Fail("Expected output to contain a single line, but got " + failMessage);

            return lines[1];
        }

        private string GetLocalCopyVersion(string localCopyVersionFile)
        {
            if (!File.Exists(localCopyVersionFile))
                return null;

            return File.ReadAllText(localCopyVersionFile);
        }

        private void DownloadLocalCopy(string nugetExe, string sourceOption, string localCopyFolder, string localCopyVersionFile, string deployedVersion)
        {
            // the internet has a more up-to-date copy, so download that

            if (Directory.Exists(localCopyFolder))
                Directory.Delete(localCopyFolder, true);

            foreach (var nuspecFile in Directory.GetFiles("..\\..\\..\\", "Lucid.*.nuspec", SearchOption.AllDirectories))
            {
                var packageName = Path.GetFileNameWithoutExtension(nuspecFile);

                Run.Program(nugetExe, string.Format("install {0} -o \"{1}\"" + sourceOption, packageName, localCopyFolder));
            }

            File.WriteAllText(localCopyVersionFile, deployedVersion);
        }

        private string GetThisVersion()
        {
            var assembly = GetType().Assembly;
            var version = assembly.GetName().Version;
            return version.ToString();
        }
    }
}
