using System.Net;
using System.Web.Mvc;
using Demo.Web.App.Generation;
using Demo.Web.ProjectCreation;
using Demo.Web.Tests.Utility;
using FluentAssertions;
using MvcTesting.Html;
using NUnit.Framework;

namespace Demo.Web.Tests.App
{
    public class GenerationTests : WebTest
    {
        [Test]
        public void Generate_GET_RendersForm()
        {
            WebAppTest(client =>
            {
                var response = client.Get(Actions.Generate());
                var form = response.Form<GenerateProject>();

                form.Method.Should().Be("post");
                form.Action.Should().BeAction(Actions.Generate());

                form.GetText(m => m.Name).Should().Be("Demo");
            });
        }

        [Test]
        public void Generate_POST_ExecutesCommand()
        {
            WebAppTest(client =>
            {
                var expectedBytes = new byte[0];
                ExecutorStub.SetupCommand(new GenerateProject(), expectedBytes);

                var response = client.Get(Actions.Generate()).Form<GenerateProject>()
                    .SetText(m => m.Name, "NewProject")
                    .Submit(client, r => r.SetExpectedResponse(HttpStatusCode.OK));

                ExecutorStub.Executed<GenerateProject>(0).ShouldBeEquivalentTo(new GenerateProject
                {
                    Name = "NewProject",
                });

                response.ActionResultOf<FileContentResult>().FileContents.Should().BeSameAs(expectedBytes);
            });
        }
    }
}
