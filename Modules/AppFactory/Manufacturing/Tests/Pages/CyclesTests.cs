using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Lucid.Modules.AppFactory.Manufacturing.Tests.Pages
{
    public class CyclesTests : ModuleTestSetup.PagesTest
    {
        [Test]
        public async Task View()
        {
            var response = await Client().GetAsync("/appFactory/manufacturing/cycles");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Text.Should().Contain("Hello from Cycles page");
        }
    }
}
