using Demo.Domain.Contract.Product.Commands;
using Demo.Web.App.Planning;
using Demo.Web.Tests.Utility;
using FluentAssertions;
using NUnit.Framework;

namespace Demo.Web.Tests.App
{
    public class PlanningTests : WebTest
    {
        [Test]
        public void StartDesign_GET_RendersForm()
        {
            WebAppTest(client =>
            {
                var response = client.Get(Actions.StartDesign());

                response.Text.Should().Contain("<form");
            });
        }

        [Test]
        public void StartDesign_POST_ExecutesCommand()
        {
            WebAppTest(client =>
            {
                var response = client.Post(Actions.StartDesign(), r => r
                    .SetFormValue("Name", "web test")
                );

                ExecutorStub.Executed<StartDesign>(0).ShouldBeEquivalentTo(new StartDesign
                {
                    Name = "web test",
                });
            });
        }
    }
}
