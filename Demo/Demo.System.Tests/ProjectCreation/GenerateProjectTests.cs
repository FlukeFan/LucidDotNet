using Demo.Web.ProjectCreation;
using FluentAssertions;
using NUnit.Framework;

namespace Demo.System.Tests.ProjectCreation
{
    [TestFixture]
    [Explicit("Until this is working")]
    public class GenerateProjectTests
    {
        [Test]
        public void Execute()
        {
            var cmd = new GenerateProject { Name = "ShinyNewProject" };

            var zipBytes = cmd.Execute();

            zipBytes.Should().NotBeNull();
            zipBytes.Length.Should().BeGreaterThan(0);
        }
    }
}
