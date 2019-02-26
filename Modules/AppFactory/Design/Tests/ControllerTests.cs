using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Lucid.Modules.AppFactory.Design.Tests
{
    public class ControllerTests : ModuleTestSetup.ControllerTest
    {
        [Test]
        public async Task CanSeeListOfBlueprints()
        {
            var response = await MvcTestingClient()
                .GetAsync(Actions.List());

            response.Doc.Find("#blueprintList").Should().NotBeNull();
        }
    }
}
