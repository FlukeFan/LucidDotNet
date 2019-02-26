using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Lucid.Modules.AppFactory.Design.Blueprints;
using NUnit.Framework;

namespace Lucid.Modules.AppFactory.Design.Tests.Blueprints
{
    public class ControllerTests : ModuleTestSetup.ControllerTest
    {
        [Test]
        public async Task CanSeeListOfBlueprints()
        {
            ExecutorStub.StubResult<FindBlueprintsQuery>(new List<Blueprint>
            {
                new BlueprintBuilder().Value(),
                new BlueprintBuilder().Value(),
            });

            var response = await MvcTestingClient()
                .GetAsync(Actions.List());

            response.Doc.Find(".blueprintList").Should().NotBeNull();
            response.Doc.FindAll(".blueprintItem").Count.Should().Be(2);
        }
    }
}
