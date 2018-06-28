using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using FluentAssertions;
using Lucid.Web.ProjectCreation;
using NUnit.Framework;

namespace Lucid.Web.Tests.ProjectCreation
{
    [TestFixture]
    public class GenerateTests
    {
        [Test]
        public void FormatIsMaintained()
        {
            var rootPath = @"..\..\..";
            var cmd = new GenerateProject { Name = "Lucid" };
            var bytes = cmd.Execute();

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
                "DemoProj.Web\\Project.zip", // this is generated in the 'BeforeBuild'
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

            using (var zipInputStream = assembly.GetManifestResourceStream("Lucid.Web.Project.zip"))
            using (var zipFile = new ZipArchive(zipInputStream))
                foreach (var zipEntry in zipFile.Entries)
                {
                    var fileName = zipEntry.FullName;

                    if (fileName.EndsWith("ProjectCreation/Generate.cs"))
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

            var extraGuids = Generate.GuidsToIgnore
                .Union(Generate.GuidsToReplace)
                .Where(g => !allGuidsInZip.Contains(g))
                .ToList();

            extraGuids.Count.Should().Be(0, "GUIDs not found in project: {0}", string.Join(", ", extraGuids));
        }

        [Test]
        public void GuidsDontAppearTwice()
        {
            var duplicateGuids = Generate.GuidsToIgnore
                .Where(g => Generate.GuidsToReplace.Contains(g))
                .ToList();

            duplicateGuids.Count.Should().Be(0, "The following GUIDs should not appear twice: {0}", string.Join(", ", duplicateGuids));
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
