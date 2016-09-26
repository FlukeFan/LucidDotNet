using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using FluentAssertions;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using Lucid.Web.ProjectCreation;
using NUnit.Framework;

namespace Lucid.System.Tests.ProjectCreation
{
    [TestFixture]
    [Explicit("Very slow system test - this is run explicitly before pushing to production")]
    public class GenerateProjectTests
    {
        [Test]
        public void Execute()
        {
            var cmd = new GenerateProject { Name = "ShinyNewProject1" };

            var zipBytes = cmd.Execute();

            var originalFolder = @"..\..\..";
            var buildFolder = Path.Combine(Path.GetTempPath(), "GpTmp");
            Console.WriteLine("Building project {0} in {1}", cmd.Name, buildFolder);
            Unzip(zipBytes, buildFolder);
            CopyFolder(@"..\..\..\packages", Path.Combine(buildFolder, "packages"));
            var currentBuildEnvironment = File.ReadAllText(Path.Combine(originalFolder, "_items/BuildEnvironment.json"));
            File.WriteAllText(Path.Combine(buildFolder, "_items/BuildEnvironment.json"), currentBuildEnvironment.Replace("Lucid", "ShinyNewProject1"));

            var fxFolder = @"C:\Program Files (x86)\MSBuild\14.0\Bin\";
            var setupCmd = File.ReadAllText(Path.Combine(buildFolder, "CommandPrompt.bat"));
            setupCmd.Should().Contain(fxFolder);

            var lastReadmeCharBefore = LastCharOfFile(Path.Combine(originalFolder, "readme.txt"));
            var lastReadmeCharAfter = LastCharOfFile(Path.Combine(buildFolder, "readme.txt"));

            lastReadmeCharBefore.Should().Be(10);
            lastReadmeCharAfter.Should().Be(lastReadmeCharBefore, "trailing lines in readme.txt should be maintained (and last line should be empty)");

            File.ReadAllText(Path.Combine(buildFolder, "ShinyNewProject1.Web\\ShinyNewProject1.Web.csproj")).Should().Contain("349c5851-65df-11da-9384-00065b846f21", "ProjectTypeGuids should not be replaced");

            var processedFiles = Directory.EnumerateFiles(buildFolder, "*.*", SearchOption.AllDirectories)
                .Where(d => Generate.ShouldProcessFile(d));

            foreach (var processedFile in processedFiles)
                foreach (var line in File.ReadAllLines(processedFile))
                    if (Path.GetFileName(processedFile) != "GenerateProjectTests.cs" && line.ToLower().Contains("Lucid"))
                        Assert.Fail("Found [Dd]emo in {0}: {1}", processedFile, line);

            var originalGuids = FindGuids(originalFolder);
            var newGuids = FindGuids(buildFolder);
            newGuids.Count.Should().BeGreaterThan(1);

            foreach (var originalGuid in originalGuids)
                if (newGuids.Contains(originalGuid))
                    Assert.Fail("GUID {0} was found in both the original project and the generated project", originalGuid);

            var msBuild = Path.Combine(fxFolder, "msbuild.exe");
            RunVerify(msBuild, "ShinyNewProject1.proj", buildFolder);

            // Assert DB?  NUnit logs?

            RunVerify(msBuild, "ShinyNewProject1.proj /t:clean", buildFolder);
            DeleteFolder(buildFolder, 5);
        }

        private int LastCharOfFile(string path)
        {
            using (var stream = File.OpenRead(path))
            {
                stream.Seek(-1, SeekOrigin.End);
                return stream.ReadByte();
            }
        }

        private IList<string> FindGuids(string folder)
        {
            var guids = new List<string>();

            var projectFiles = Directory.EnumerateFiles(folder, "*.csproj", SearchOption.AllDirectories);
            var slnFiles = Directory.EnumerateFiles(folder, "*.sln", SearchOption.AllDirectories);
            var files = projectFiles.Union(slnFiles);

            foreach (var file in files)
            {
                if (!Generate.ShouldProcessFile(file))
                    continue;

                var lines = File.ReadAllLines(file);
                foreach (var line in lines)
                    foreach (Match match in Generate.GuidSearch.Matches(line))
                    {
                        var guid = match.Value.ToUpper();

                        if (match.Success && !guids.Contains(guid) && !Generate.GuidsToIgnore.Contains(guid))
                            guids.Add(guid);
                    }
            }

            return guids;
        }

        private void CopyFolder(string sourceFolder, string destinationFolder)
        {
            foreach (var dir in Directory.GetDirectories(sourceFolder, "*", SearchOption.AllDirectories))
                Directory.CreateDirectory(dir.Replace(sourceFolder, destinationFolder));

            foreach (var file in Directory.GetFiles(sourceFolder, "*.*", SearchOption.AllDirectories))
                File.Copy(file, file.Replace(sourceFolder, destinationFolder), true);
        }

        private void RunVerify(string fileName, string arguments, string folder)
        {
            var result = Run.Program(fileName, arguments, folder);

            if (result.ExitCode != 0)
            {
                foreach (var outputLine in result.Output)
                    Console.WriteLine(outputLine);

                foreach (var errorLine in result.Errors)
                    Console.Error.WriteLine(errorLine);

                var failMessage = string.Format("Unexpected ExitCode {0} running {1} {2} in {3}", result.ExitCode, fileName, arguments, folder);
                Console.WriteLine(failMessage);
                Assert.Fail(failMessage);
            }
        }

        private void Unzip(byte[] zipBytes, string folder)
        {
            if (Directory.Exists(folder))
                DeleteFolder(folder, 5);

            var buffer = new byte[4096];
            using (var ms = new MemoryStream(zipBytes))
            using (var zipFile = new ZipFile(ms))
                foreach (ZipEntry zipEntry in zipFile)
                {
                    var outFile = Path.Combine(folder, zipEntry.Name);
                    Directory.CreateDirectory(Path.GetDirectoryName(outFile));

                    using (var streamWriter = File.Create(outFile))
                        StreamUtils.Copy(zipFile.GetInputStream(zipEntry), streamWriter, buffer);
                }
        }

        private static void DeleteFolder(string path, int retryCount)
        {
            try
            {
                Directory.Delete(path, true);
            }
            catch
            {
                // *sigh* http://stackoverflow.com/questions/329355/cannot-delete-directory-with-directory-deletepath-true#comment11564214_329502

                if (retryCount <= 0)
                    throw;

                Thread.Sleep(3);
                DeleteFolder(path, retryCount - 1);
            }
        }
    }
}
