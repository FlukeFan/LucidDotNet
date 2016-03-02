using System.Net;
using Demo.Web.App.Generation;
using Demo.Web.ProjectCreation;
using Demo.Web.Tests.Utility;
using FluentAssertions;
using Lucid.Web.Testing.Html;
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

                form.Element.Attribute("method").Should().Be("post");
                form.Element.Attribute("action").Should().BeAction(Actions.Generate());

                form.GetText(m => m.Name).Should().Be("Demo");
            });
        }

        [Test]
        public void Generate_POST_ExecutesCommand()
        {
            WebAppTest(client =>
            {
                ExecutorStub.SetupCommand(new GenerateProject(), new byte[0]);

                var form = client.Get(Actions.Generate()).Form<GenerateProject>()
                    .SetText(m => m.Name, "NewProject");

                var response = client.Post(HttpStatusCode.OK, Actions.Generate(), r => form.SetFormValues(r));

                ExecutorStub.Executed<GenerateProject>(0).ShouldBeEquivalentTo(new GenerateProject
                {
                    Name = "NewProject",
                });
            });
        }
    }
}
