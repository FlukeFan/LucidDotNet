using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace Lucid.Infrastructure.Host.Web.Tests
{
    [TestFixture]
    public class ViewTests : WebTests.Controller
    {
        [Test]
        public async Task CanSeeModuleViews()
        {
            var response = await MvcTestingClient().GetAsync(Modules.ProjectCreation.Actions.Index());

            response.HttpStatusCode.Should().Be(HttpStatusCode.OK);
            response.LastResult.Should().BeOfType<ViewResult>();
        }

        [Test]
        public async Task CanSeeModuleViewsWithSameViewName()
        {
            var response = await MvcTestingClient().GetAsync(Modules.Temp.Actions.Index());

            response.HttpStatusCode.Should().Be(HttpStatusCode.OK);
            response.LastResult.Should().BeOfType<ViewResult>();
        }

    }
}
