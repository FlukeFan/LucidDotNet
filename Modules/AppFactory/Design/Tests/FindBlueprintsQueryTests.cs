using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using Reposify.Testing;

namespace Lucid.Modules.AppFactory.Design.Tests
{
    [TestFixture]
    public class FindBlueprintsQueryTests : ModuleTestSetup.LogicTest
    {
        [Test]
        public async Task OrdersByName()
        {
            await new BlueprintBuilder().With(bp => bp.Name, "Bp3").SaveAsync(Registry.Repository.Value);
            await new BlueprintBuilder().With(bp => bp.Name, "Bp1").SaveAsync(Registry.Repository.Value);
            await new BlueprintBuilder().With(bp => bp.Name, "Bp2").SaveAsync(Registry.Repository.Value);

            var blueprints = await new FindBlueprintsQuery().ExecuteAsync();

            blueprints.Select(bp => bp.Name).Should().Equal("Bp1", "Bp2", "Bp3");
        }
    }
}
