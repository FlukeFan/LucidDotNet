using System.Web.Mvc;
using Lucid.Domain.Contract.Product.Commands;
using Lucid.Web.App.Planning;
using Lucid.Web.Tests.Utility;
using FluentAssertions;
using MvcTesting.Html;
using NUnit.Framework;

namespace Lucid.Web.Tests.App
{
    public class PlanningTests : WebTest
    {
        [Test]
        public void List_GET_RendersLinks()
        {
            WebAppTest(client =>
            {
                var response = client.Get(Actions.List());

                response.Doc.Find("#designList").Should().NotBeNull();
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

                response.ActionResultOf<RedirectResult>().Url.Should().Be(Actions.List());
            });
        }
    }
}
