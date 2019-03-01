using System.Net;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Lucid.Infrastructure.Lib.Testing.Controller;
using Lucid.Infrastructure.Lib.Testing.Execution;
using Microsoft.AspNetCore.Mvc;
using MvcTesting.Html;
using NUnit.Framework;

namespace Lucid.Modules.ProjectCreation.Tests
{
    public class ControllerTests : ModuleTestSetup.ControllerTest
    {
        [Test]
        public async Task Can_SeeProjectCreationForm()
        {
            var form = await MvcTestingClient()
                .GetAsync(Actions.Index())
                .Form<GenerateProjectCommand>();

            form.Method.Should().Be("post");
            form.Action.Should().Be(Actions.Index().PrefixLocalhost());

            form.GetText(m => m.Name).Should().Be("Demo", "default project generation name should be 'Demo'");
        }

        [Test]
        public async Task Can_DownloadProjectZipFile()
        {
            var expectedByteString = "stub bytes";
            ExecutorStub.StubResult<GenerateProjectCommand>(Encoding.ASCII.GetBytes(expectedByteString));

            var form = await MvcTestingClient()
                .GetAsync(Actions.Index())
                .Form<GenerateProjectCommand>();

            var response = await form
                .SetText(m => m.Name, "NewProj_1")
                .Submit(r => r.SetExpectedResponse(HttpStatusCode.OK));

            ExecutorStub.VerifySingleExecuted(Agreements.GenerateProject);

            var fileResult = response.ActionResultOf<FileResult>();
            fileResult.ContentType.Should().Be("application/zip");
            fileResult.FileDownloadName.Should().Be("NewProj_1.zip");

            response.Text.Should().Be(expectedByteString);
        }
    }
}
