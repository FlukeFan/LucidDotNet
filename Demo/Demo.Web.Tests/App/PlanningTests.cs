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
            WebApp.Test(client =>
            {
                var response = client.Get(Actions.StartDesign());

                response.Should().Contain("<form");
            });
        }

        [Test]
        public void StartDesign_POST_ExecutesCommand()
        {
            WebApp.Test(client =>
            {
                var response = client.Post(Actions.StartDesign());

                response.Should().Contain("stub out command executor");
            });
        }
    }
}
