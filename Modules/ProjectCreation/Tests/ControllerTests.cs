using System.Net;
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
        public async Task Root_RedirectsToProjectCreation()
        {
            var response = await MvcTestingClient().GetAsync("/");

            response.StatusCode.Should().Be(HttpStatusCode.Redirect);
            response.ActionResultOf<RedirectResult>().Url.Should().Be("projectCreation");
        }

        [Test]
        public async Task Index_DisplaysForm()
        {
            var form = await MvcTestingClient()
                .GetAsync("/ProjectCreation")
                .Form<IndexModel>();

            form.Method.Should().Be("post");
            form.Action.Should().Be("/projectCreation");

            form.GetText(m => m.Cmd.Name).Should().Be("Demo", "default project generation name should be 'Demo'");
        }

        [Test]
        public async Task Index_Post_GeneratesProject()
        {
            var expectedByteString = "stub bytes";
            ExecutorStub.StubResult<GenerateProject>(Encoding.ASCII.GetBytes(expectedByteString));

            var form = await MvcTestingClient()
                .GetAsync("/ProjectCreation")
                .Form<IndexModel>();

            var response = await form
                .SetText(m => m.Cmd.Name, "PostTest")
                .Submit();

            ExecutorStub.SingleExecuted<GenerateProject>().Should().BeEquivalentTo(new GenerateProject { Name = "PostTest" });

            var fileResult = response.ActionResultOf<FileResult>();
            fileResult.ContentType.Should().Be("application/zip");
            fileResult.FileDownloadName.Should().Be("PostTest.zip");

            response.Text.Should().Be(expectedByteString);
        }
    }
}
