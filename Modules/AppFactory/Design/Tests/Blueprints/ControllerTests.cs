using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Lucid.Infrastructure.Lib.Facade.Exceptions;
using Lucid.Infrastructure.Lib.Testing.Controller;
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
            response.Doc.Find(".blueprintItem:nth-child(1) a").Attribute("href").Should().StartWith(Actions.Edit(101));
        }

        [Test]
        public async Task Can_StartBlueprint()
        {
            var client = MvcTestingClient();

            var getResponse = await client
                .GetAsync(Actions.Start());

            var model = getResponse.ViewResultModel<StartEditModel>();
            model.Title.Should().Be("Start Blueprint");
            model.ButtonText.Should().Be("Start Blueprint");

            var response = await getResponse.Form<StartEditCommand>()
                .SetText(m => m.Name, "TestBlueprint")
                .Submit();

            ExecutorStub.VerifySingleExecuted(Agreements.Start);
        }

        [Test]
        public async Task Can_EditBlueprint()
        {
            ExecutorStub.StubResult(Agreements.FindBlueprint);

            var client = MvcTestingClient();

            var getResponse = await client
                .GetAsync(Actions.Edit(123));

            var model = getResponse.ViewResultModel<StartEditModel>();
            model.Title.Should().Be("Edit Blueprint");
            model.ButtonText.Should().Be("Update");

            var form = getResponse.Form<StartEditCommand>();
            form.GetText(m => m.Name).Should().Be("TestBlueprint");

            var response = await form
                .SetText(m => m.Name, "UpdatedBlueprint")
                .Submit();

            ExecutorStub.VerifySingleExecuted(Agreements.Edit);
        }

        [Test]
        public async Task WhenError_RedisplaysPage()
        {
            ExecutorStub.StubResult<StartEditCommand>(l => throw new FacadeException("simulated error"));

            var form = await MvcTestingClient().GetAsync(Actions.Start()).Form<StartEditCommand>();
            await form.Submit(r => r.SetExpectedResponse(HttpStatusCode.OK));
        }
    }
}
