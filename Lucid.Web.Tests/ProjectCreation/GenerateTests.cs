using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Lucid.Web.ProjectCreation;
using FluentAssertions;
using ICSharpCode.SharpZipLib.Zip;
using NUnit.Framework;

namespace Lucid.Web.Tests.ProjectCreation
{
    [TestFixture]
    public class GenerateTests
    {
        [Test]
        public void GuidsAreUsedInProject()
        {
            var assembly = typeof(Generate).Assembly;
            var allGuidsInZip = new List<string>();
            var ignoredGenerateCs = false;

            using (var zipInputStream = assembly.GetManifestResourceStream("Demo.Web.Project.zip"))
            using (var zipFile = new ZipFile(zipInputStream))
                foreach (ZipEntry zipEntry in zipFile)
                {
                    var fileName = zipEntry.Name;

                    if (fileName.EndsWith("ProjectCreation/Generate.cs"))
                    {
                        ignoredGenerateCs = true;
                        continue;
                    }

                    if (!Generate.ShouldProcessFile(fileName))
                        continue;

                    var lines = Generate.ReadLines(zipFile.GetInputStream(zipEntry));

                    foreach (var line in lines)
                        foreach (Match match in Generate.GuidSearch.Matches(line))
                            allGuidsInZip.Add(match.Value.ToUpper());
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
    }
}
