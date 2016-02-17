using System.Net;
using Demo.Web.App.Generation;
using Demo.Web.ProjectCreation;
using Demo.Web.Tests.Utility;
using FluentAssertions;
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

                response.Doc.Find("form").Where(f =>
                {
                    f.Attribute("method").Should().Be("post");
                    f.Attribute("action").Should().BeAction(Actions.Generate());
                });
            });
        }

        [Test]
        public void Generate_POST_ExecutesCommand()
        {
            WebAppTest(client =>
            {
                ExecutorStub.SetupCommand(new GenerateProject(), new byte[0]);

                var response = client.Post(HttpStatusCode.OK, Actions.Generate(), r => r
                    .SetFormValue("Name", "NewProject")
                );

                ExecutorStub.Executed<GenerateProject>(0).ShouldBeEquivalentTo(new GenerateProject
                {
                    Name = "NewProject",
                });
            });
        }
    }
}
