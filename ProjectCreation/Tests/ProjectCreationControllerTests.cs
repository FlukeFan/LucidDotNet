using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Lucid.Infrastructure.Lib.Testing;
using NUnit.Framework;

namespace Lucid.ProjectCreation.Tests
{
    public class ProjectCreationControllerTests : ModuleControllerTests<TestStartup>
    {
        public ProjectCreationControllerTests() : base(@"..\..\..\..\Module") { }

        [Test]
        public async Task Index_DisplaysForm()
        {
            var response = await MvcTestingClient()
                .GetAsync("/ProjectCreation");

            response.HttpStatusCode.Should().Be(HttpStatusCode.OK);
            response.Text.Should().StartWith("Hello from deployed");
        }
    }
}
