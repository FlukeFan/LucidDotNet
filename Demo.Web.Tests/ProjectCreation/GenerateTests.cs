using System.Linq;
using Demo.Web.ProjectCreation;
using FluentAssertions;
using NUnit.Framework;

namespace Demo.Web.Tests.ProjectCreation
{
    [TestFixture]
    public class GenerateTests
    {
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
