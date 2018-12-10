using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using MvcTesting.Html;
using NUnit.Framework;

namespace Lucid.Modules.ProjectCreation.Tests
{
    public class ControllerTests : ModuleTest.Controller
    {
        [Test]
        public async Task Can_SeeProjectCreationForm()
        {
            var form = await MvcTestingClient()
                .GetAsync(Actions.Index())
                .Form<GenerateProject>();

            form.Method.Should().Be("post");
            form.Action.Should().Be(AddHost(Actions.Index()));

            form.GetText(m => m.Name).Should().Be("Demo", "default project generation name should be 'Demo'");
        }

        [Test]
        public async Task Can_DownloadProjectZipFile()
        {
            var expectedByteString = "stub bytes";
            ExecutorStub.StubResult<GenerateProject>(Encoding.ASCII.GetBytes(expectedByteString));

            var form = await MvcTestingClient()
                .GetAsync(Actions.Index())
                .Form<GenerateProject>();

            var response = await form
                .SetText(m => m.Name, "PostTest")
                .Submit();

            ExecutorStub.SingleExecuted<GenerateProject>().Should().BeEquivalentTo(new GenerateProject { Name = "PostTest" });

            var fileResult = response.ActionResultOf<FileResult>();
            fileResult.ContentType.Should().Be("application/zip");
            fileResult.FileDownloadName.Should().Be("PostTest.zip");

            response.Text.Should().Be(expectedByteString);
        }
    }
}
