using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using Reposify.Testing;

namespace Lucid.Modules.AppFactory.Design.Tests
{
    [TestFixture]
    public class FindBlueprintsQueryTests
    {
        [Test]
        public async Task OrdersByName()
        {
            using (var db = new ModuleTestSetup.SetupDbTx())
            {
                await new BlueprintBuilder().With(bp => bp.Name, "Bp3").SaveAsync(db.NhRepository);
                await new BlueprintBuilder().With(bp => bp.Name, "Bp1").SaveAsync(db.NhRepository);
                await new BlueprintBuilder().With(bp => bp.Name, "Bp2").SaveAsync(db.NhRepository);

                var blueprints = await new FindBlueprintsQuery().ExecuteAsync();

                blueprints.Select(bp => bp.Name).Should().Equal("Bp1", "Bp2", "Bp3");
            }
        }
    }
}
