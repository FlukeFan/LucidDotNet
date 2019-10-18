using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Lucid.Modules.AppFactory.Manufacturing.Domain.Cycles;
using MvcTesting.Html;
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
            response.Doc.Find("a[data-modal-dialog=true]").TextContent.Should().Be("Start Cycle");
        }

        [Test]
        public async Task Can_StartCycle()
        {
            ExecutorStub.StubResult<StartCycleCommand>(123);

            var client = Client();

            var getResponse = await client
                .GetAsync("/appFactory/manufacturing/cycles/start");

            var response = await getResponse.Form<StartCycleCommand>()
                .SetText("Quantity", "123")
                .Submit();

            response.Should().NotBeNull();

            ExecutorStub.SingleExecuted<StartCycleCommand>().Should().BeEquivalentTo(new StartCycleCommand
            {
                BlueprintId = -1,
                Quantity = 123,
            });
        }
    }
}
