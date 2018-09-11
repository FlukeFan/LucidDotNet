using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using FluentAssertions;
using NUnit.Framework;

namespace Lucid.System.ProjectCreation.Tests
{
    [TestFixture]
    public class GenerateTests
    {
        [Test]
        public void FormatIsMaintained()
        {
            var rootPath = FindRootPath();
            var cmd = new GenerateProject { Name = "Lucid" };
            var bytes = cmd.Execute();
            var zipCreated = File.GetLastWriteTimeUtc(Path.Combine(rootPath, "ProjectCreation/Module/Project.zip"));

            var zippedFiles = new List<string>();

            using (var ms = new MemoryStream(bytes))
            using (var zipFile = new ZipArchive(ms))
            {
                foreach (var zipEntry in zipFile.Entries)
                {
                    var name = zipEntry.FullName;

                    if (Generate.ShouldProcessFile(name))
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
                            var originalBom = Generate.ReadBomEncoding(stream);
                            var copiedBom = Generate.ReadBomEncoding(copy);

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
        public void GeneratedProjectContainsFilesReferencedByEachCsproj()
        {
            var cmd = new GenerateProject { Name = "DemoProj" };
            var bytes = cmd.Execute();

            var zippedFiles = new List<string>();
            var csProjEntries = new List<CsprojEntry>();

            using (var ms = new MemoryStream(bytes))
            using (var zipFile = new ZipArchive(ms))
            {
                foreach (var zipEntry in zipFile.Entries)
                {
                    var name = zipEntry.FullName;

                    name.ToLower().Should().NotBe("license.txt", "LICENSE.txt should not be included in output (should be part of skipped files)");

                    var zippedFileName = name.Replace("/", "\\");
                    zippedFiles.Add(zippedFileName);

                    if (name.ToLower().EndsWith(".csproj"))
                        using (var sr = new StreamReader(zipEntry.Open()))
                            csProjEntries.Add(new CsprojEntry
                            {
                                Name = Path.GetFileName(name),
                                Folder = Path.GetDirectoryName(name),
                                Content = sr.ReadToEnd(),
                            });
                }
            }

            var knownGeneratedFiles = new string[]
            {
                "ProjectCreation\\Module\\Project.zip", // this is generated in the 'BeforeBuild'
            };

            var fileElements = new string[]
            {
                "Compile",
                "None",
                "Content",
                "EmbeddedResource",
            };

            foreach (var csproj in csProjEntries)
            {
                var proj = new XmlDocument();
                proj.LoadXml(csproj.Content);

                foreach (XmlElement el in proj.SelectNodes("//*"))
                {
                    if (fileElements.Contains(el.Name) && el.HasAttribute("Include"))
                    {
                        var file = el.Attributes["Include"].Value;

                        if (file.Contains(".."))
                            continue; // ignore relative files

                        var expectedFile = Path.Combine(csproj.Folder, file);

                        if (knownGeneratedFiles.Contains(expectedFile))
                            continue;

                        expectedFile.ToLower().Should().NotEndWith("\\lucid.js", "file should have been renamed");

                        zippedFiles.Should().Contain(expectedFile,
                            "file {0} was referenced by project {1}", file, csproj.Name);
                    }
                }
            }
        }

        [Test]
        public void GuidsUsedInProjectAreKnown()
        {
            var assembly = typeof(Generate).Assembly;
            var allGuidsInZip = new List<string>();
            var ignoredGenerateCs = false;

            using (var zipInputStream = assembly.GetManifestResourceStream("Lucid.ProjectCreation.Project.zip"))
            using (var zipFile = new ZipArchive(zipInputStream))
                foreach (var zipEntry in zipFile.Entries)
                {
                    var fileName = zipEntry.FullName;

                    if (fileName.EndsWith("ProjectCreation/Module/Generate.cs"))
                    {
                        ignoredGenerateCs = true;
                        continue;
                    }

                    if (!Generate.ShouldProcessFile(fileName))
                        continue;

                    using (var stream = zipEntry.Open())
                    {
                        var lines = Generate.ReadLines(stream);

                        foreach (var line in lines)
                            foreach (Match match in Generate.GuidSearch.Matches(line))
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

            var outputLine = Generate.ProcessLine(inputLine, "NewProj1");

            outputLine.Should().Be("DoesNotNeedChanging");
        }

        [Test]
        public void ProcessLine_RemovesTempDirFromWebConfig()
        {
            var inputLine = "    <compilation debug=\"true\" targetFramework=\"4.5\" optimizeCompilations=\"true\" tempDirectory=\"D:\\temp\\a4vy1sad.evu\\temp\\\" />";

            var outputLine = Generate.ProcessLine(inputLine, "NewProj1");

            outputLine.Should().Be("    <compilation debug=\"true\" targetFramework=\"4.5\" optimizeCompilations=\"true\"  />");
        }

        public class CsprojEntry
        {
            public string Name;
            public string Folder;
            public string Content;
        }
    }
}
