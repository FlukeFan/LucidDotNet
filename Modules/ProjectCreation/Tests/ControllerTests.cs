using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Lucid.Infrastructure.Lib.Testing;
using NUnit.Framework;

namespace Lucid.Modules.ProjectCreation.Tests
{
    public class ControllerTests : ModuleControllerTests<TestStartup>
    {
        [Test]
        public async Task Index_DisplaysForm()
        {
            var response = await MvcTestingClient()
                .GetAsync("/ProjectCreation");

            response.HttpStatusCode.Should().Be(HttpStatusCode.OK);
            response.Text.Should().Contain("Hello from deployed");
            response.Text.Should().Contain("precompiled");
        }
    }
}
