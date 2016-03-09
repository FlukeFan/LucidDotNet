using Demo.Domain.Contract.Product.Commands;
using Demo.Web.App.Planning;
using Demo.Web.Tests.Utility;
using FluentAssertions;
using Lucid.Web.Testing.Html;
using NUnit.Framework;

namespace Demo.Web.Tests.App
{
    public class PlanningTests : WebTest
    {
        [Test]
        public void List_GET_RendersLinks()
        {
            WebAppTest(client =>
            {
                var response = client.Get(Actions.List());

                response.Doc.Find("ul").Should().NotBeNull();
            });
        }

        [Test]
        public void StartDesign_GET_RendersForm()
        {
            WebAppTest(client =>
            {
                var get = client.Get(Actions.StartDesign());
                var form = get.Form<StartDesign>();

                form.GetText(m => m.Name).Should().Be("");
            });
        }

        [Test]
        public void StartDesign_POST_ExecutesCommand()
        {
            WebAppTest(client =>
            {
                var get = client.Get(Actions.StartDesign());
                var form = get.Form<StartDesign>();

                var response = form
                    .SetText(m => m.Name, "web test")
                    .Submit(client);

                ExecutorStub.Executed<StartDesign>(0).ShouldBeEquivalentTo(new StartDesign
                {
                    Name = "web test",
                });

                // TODO: verify post is redirect to Actions.List()
            });
        }
    }
}
