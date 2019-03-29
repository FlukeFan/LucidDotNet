using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Lucid.Infrastructure.Lib.Facade.Pledge;
using Lucid.Infrastructure.Lib.Testing.Execution;
using Lucid.Modules.AppFactory.Design.Blueprints;
using Lucid.Modules.AppFactory.Design.Contract;
using NUnit.Framework;

namespace Lucid.Modules.AppFactory.Design.Tests.Contract
{
    [TestFixture]
    public class FindAvailableBlueprintsTests : ModuleTestSetup.LogicTest
    {
        [Test]
        public async Task VerifyAgreement()
        {
            var agreement = Agreements.FindAvailableBlueprints;

            var bp1Id = await new StartEditCommand { OwnedByUserId = Defaults.UserId, Name = "Blueprint1" }.ExecuteAsync();
            var bp2Id = await new StartEditCommand { OwnedByUserId = Defaults.UserId, Name = "Blueprint2" }.ExecuteAsync();

            var blueprints = await Registry.ExecutorAsync.ExecAsync(agreement.Executable());

            blueprints.Should().BeEquivalentTo(agreement.Result().Mutate(r =>
            {
                r[0].Mutate(bp => bp.Id = bp1Id);
                r[1].Mutate(bp => bp.Id = bp2Id);
            }));
        }

        [Test]
        public async Task OrdersByName()
        {
            await new StartEditCommand { OwnedByUserId = Defaults.UserId, Name = "Blueprint3" }.ExecuteAsync();
            await new StartEditCommand { OwnedByUserId = Defaults.UserId, Name = "Blueprint1" }.ExecuteAsync();
            await new StartEditCommand { OwnedByUserId = Defaults.UserId, Name = "Blueprint2" }.ExecuteAsync();

            var blueprints = await Registry.ExecutorAsync.ExecAsync(new FindAvailableBlueprints { UserId = Defaults.UserId });

            blueprints.Select(bp => bp.Name).Should().Equal("Blueprint1", "Blueprint2", "Blueprint3");
        }
    }
}
