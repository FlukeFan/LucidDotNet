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
                var response = client.Get("/planning/startdesign");

                response.Should().Contain("<form");
            });
        }

        [Test]
        public void StartDesign_POST_ExecutesCommand()
        {
            WebApp.Test(client =>
            {
                // TODO implement client.Post(...)
            });
        }
    }
}
