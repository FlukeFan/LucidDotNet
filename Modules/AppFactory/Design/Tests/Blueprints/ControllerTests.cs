using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Lucid.Infrastructure.Lib.Facade.Exceptions;
using Lucid.Infrastructure.Lib.Testing.Execution;
using Lucid.Modules.AppFactory.Design.Blueprints;
using MvcTesting.Html;
using NUnit.Framework;

namespace Lucid.Modules.AppFactory.Design.Tests.Blueprints
{
    public class ControllerTests : ModuleTestSetup.ControllerTest
    {
        [Test]
        public async Task CanSeeListOfBlueprints()
        {
            ExecutorStub.StubResult(Agreements.FindBlueprints);

            var response = await MvcTestingClient()
                .GetAsync(Actions.List());

            ExecutorStub.VerifySingleExecuted(Agreements.FindBlueprints);

            response.Doc.Find(".blueprintList").Should().NotBeNull();
            response.Doc.FindAll(".blueprintItem").Count.Should().Be(2);
        }

        [Test]
        public async Task Can_StartBlueprint()
        {
            var client = MvcTestingClient();

            var form = await client
                .GetAsync(Actions.Start())
                .Form<StartCommand>();

            var response = await form
                .SetText(m => m.Name, "Blueprint1")
                .Submit();

            ExecutorStub.VerifySingleExecuted(Agreements.Start);
        }

        [Test]
        public async Task WhenError_RedisplaysPage()
        {
            ExecutorStub.StubResult<StartCommand>(l => throw new FacadeException("simulated error"));

            var form = await MvcTestingClient().GetAsync(Actions.Start()).Form<StartCommand>();
            await form.Submit(r => r.SetExpectedResponse(HttpStatusCode.OK));
        }
    }
}
