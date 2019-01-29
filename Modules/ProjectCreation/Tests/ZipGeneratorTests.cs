using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Lucid.Modules.ProjectCreation.Tests
{
    [TestFixture]
    public class ZipGeneratorTests
    {
        [Test]
        public async Task FormatIsMaintained()
        {
            var rootPath = FindRootPath();
            var cmd = new GenerateProject { Name = "Lucid" };
            var bytes = await cmd.Execute();
            var zipCreated = File.GetLastWriteTimeUtc(Path.Combine(rootPath, "Modules/ProjectCreation/Module/Project.zip"));

            var zippedFiles = new List<string>();

            using (var ms = new MemoryStream(bytes))
            using (var zipFile = new ZipArchive(ms))
            {
                foreach (var zipEntry in zipFile.Entries)
                {
                    var name = zipEntry.FullName;

                    if (ZipGenerator.ShouldProcessFile(name))
                    {
                        var originalFile = Path.Combine(rootPath, name);

                        if (!File.Exists(originalFile))
                        {
                            TestContext.Progress.WriteLine($"Skip checks on file not-found: {originalFile}");
                            continue;
                        }

                        var originalLastWrite = File.GetLastWriteTimeUtc(originalFile);

                        if (originalLastWrite > zipCreated)
                        {
                            TestContext.Progress.WriteLine($"Skip checks on file {originalFile} as it is more recent {originalLastWrite} than zip {zipCreated}.");
                            continue;
                        }

                        using (var stream = File.OpenRead(originalFile))
                        using (var copy = zipEntry.Open())
                        {
                            var originalBom = ZipGenerator.ReadBomEncoding(stream);
                            var copiedBom = ZipGenerator.ReadBomEncoding(copy);

                            copiedBom.Should().Be(originalBom, $"BOM of file {name} is not the same in the generated project");
                        }

                        using (var copy = zipEntry.Open())
                        using (var streamReader = new StreamReader(copy))
                        {
                            var copyContent = streamReader.ReadToEnd();
                            var original = File.ReadAllText(originalFile);

                            var copyLfCount = copyContent.Count(c => c == '\n');
                            var originalLfCount = original.Count(c => c == '\n');

                            copyLfCount.Should().Be(originalLfCount, $"{originalFile} has {originalLfCount} line-feeds, so copy should have same amount");

                            var copyCrCount = copyContent.Count(c => c == '\r');
                            var originalCrCount = original.Count(c => c == '\r');

                            copyCrCount.Should().Be(originalCrCount, $"{originalFile} has {originalCrCount} carriage returns, so copy should have same amount");
                        }
                    }
                }
            }
        }

        private string FindRootPath()
        {
            var path = Environment.CurrentDirectory;
            var fileToFind = ".gitignore";

            while (!File.Exists(Path.Combine(path, fileToFind)) && path != Path.GetPathRoot(path))
            {
                path = Path.Combine(path, "..");
                path = Path.GetFullPath(path);
            }

            if (path == Path.GetPathRoot(path))
                Assert.Fail($"Could not find {fileToFind} in parent of {Environment.CurrentDirectory}");

            return path;
        }

        [Test]
        public async Task GeneratedProjectContainsExpectedFiles()
        {
            var cmd = new GenerateProject { Name = "DemoProj" };
            var bytes = await cmd.Execute();

            var zippedFiles = new List<string>();

            using (var ms = new MemoryStream(bytes))
            using (var zipFile = new ZipArchive(ms))
            {
                foreach (var zipEntry in zipFile.Entries)
                {
                    var name = zipEntry.FullName;

                    name.ToLower().Should().NotBe("license.txt", "LICENSE.txt should not be included in output (should be part of skipped files)");
                    name.ToLower().Should().NotContain("ftphost", $"Host information should not be included (file {name} was zipped into output)");

                    var zippedFileName = name.Replace("/", "\\");
                    zippedFiles.Add(zippedFileName);
                }
            }

            // verify a selection of files

            // root files
            zippedFiles.Should().Contain(".gitignore");
            zippedFiles.Should().Contain("build.proj");
            zippedFiles.Should().Contain("CommandPrompt.bat");
            zippedFiles.Should().Contain("global.json");
            zippedFiles.Should().Contain("DemoProj.sln");
            zippedFiles.Should().Contain("readme.md");
            zippedFiles.Should().Contain("docker-compose.yml");

            // build files
            zippedFiles.Should().Contain("Build\\App_Offline.htm");
            zippedFiles.Should().Contain("Build\\common.build.proj");
            zippedFiles.Should().Contain("Build\\common.targets");
            zippedFiles.Should().Contain("Build\\BuildUtil\\Build.BuildUtil.csproj");
            zippedFiles.Should().Contain("Build\\BuildUtil\\Program.cs");

            // Host files
            zippedFiles.Should().Contain("Infrastructure\\Host\\Web\\bundleconfig.json");
            zippedFiles.Should().Contain("Infrastructure\\Host\\Web\\compilerconfig.json.defaults");
            zippedFiles.Should().Contain("Infrastructure\\Host\\Web\\nlog.config");
            zippedFiles.Should().Contain("Infrastructure\\Host\\Web\\Content\\demoProj.js");
            zippedFiles.Should().Contain("Infrastructure\\Host\\Web\\Content\\web.scss");
            zippedFiles.Should().Contain("Infrastructure\\Host\\Web\\Home\\Index.cshtml");
            zippedFiles.Should().Contain("Infrastructure\\Host\\Web\\wwwroot\\favicon.ico");
            zippedFiles.Should().NotContain("Infrastructure\\Host\\Web\\wwwroot\\css\\site.min.css");
            zippedFiles.Should().NotContain("Infrastructure\\Host\\Web\\wwwroot\\js\\site.min.js");
            zippedFiles.Should().NotContain("Infrastructure\\Host\\Web.Tests\\wwwroot\\DemoProj.Infrastructure.Host.Web.Tests.csproj.user");

            // lib files
            zippedFiles.Should().NotContain("Infrastructure\\Lib\\MvcApp\\wwwroot\\lib\\mvcForms\\css\\mvcForms.css");

            // ProjectCreation module
            zippedFiles.Should().Contain("Modules\\ProjectCreation\\Module\\DemoProj.Modules.ProjectCreation.csproj");
            zippedFiles.Should().Contain("Modules\\ProjectCreation\\Module\\Zip.proj");
            zippedFiles.Should().NotContain("Modules\\ProjectCreation\\Module\\Project.zip");
            zippedFiles.Should().NotContain("Modules\\ProjectCreation\\Module\\Project.zip.nextUpdate");
            zippedFiles.Should().NotContain("Modules\\ProjectCreation\\Module\\bin\\Debug\\netstandard2.0\\DemoProj.Modules.ProjectCreation.dll");
            zippedFiles.Should().NotContain("Modules\\ProjectCreation\\Module\\obj\\project.assets.json");
        }

        [Test]
        public void GuidsUsedInProjectAreKnown()
        {
            var assembly = typeof(ZipGenerator).Assembly;
            var allGuidsInZip = new List<string>();
            var ignoredGenerateCs = false;

            using (var zipInputStream = assembly.GetManifestResourceStream("Lucid.Modules.ProjectCreation.Project.zip"))
            using (var zipFile = new ZipArchive(zipInputStream))
                foreach (var zipEntry in zipFile.Entries)
                {
                    var fileName = zipEntry.FullName;

                    if (fileName.EndsWith("ProjectCreation/Module/ZipGenerator.cs"))
                    {
                        ignoredGenerateCs = true;
                        continue;
                    }

                    if (!ZipGenerator.ShouldProcessFile(fileName))
                        continue;

                    using (var stream = zipEntry.Open())
                    {
                        var lines = ZipGenerator.ReadLines(stream);

                        foreach (var line in lines)
                            foreach (Match match in ZipGenerator.GuidSearch.Matches(line))
                                allGuidsInZip.Add(match.Value.ToUpper());
                    }
                }

            ignoredGenerateCs.Should().BeTrue("should have found Generate.cs");

            foreach (var knownGuid in Guids.ToIgnore)
                allGuidsInZip.Should().Contain(knownGuid, $"GUID {knownGuid} should remain in project");
        }

        [Test]
        public void ProcessLine_Unaffected()
        {
            var inputLine = "DoesNotNeedChanging";

            var outputLine = ZipGenerator.ProcessLine(inputLine, "NewProj1");

            outputLine.Should().Be("DoesNotNeedChanging");
        }

        [Test]
        public void ProcessLine_RemovesTempDirFromWebConfig()
        {
            var inputLine = "    <compilation debug=\"true\" targetFramework=\"4.5\" optimizeCompilations=\"true\" tempDirectory=\"D:\\temp\\a4vy1sad.evu\\temp\\\" />";

            var outputLine = ZipGenerator.ProcessLine(inputLine, "NewProj1");

            outputLine.Should().Be("    <compilation debug=\"true\" targetFramework=\"4.5\" optimizeCompilations=\"true\"  />");
        }
    }
}
