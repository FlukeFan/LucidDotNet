using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Lucid.Infrastructure.Lib.Testing.Execution;
using Lucid.Modules.AppFactory.Design.Blueprints;
using NUnit.Framework;
using Reposify.Testing;

namespace Lucid.Modules.AppFactory.Design.Tests.Blueprints
{
    [TestFixture]
    public class FindBlueprintsQueryTests
    {
        private ModuleTestSetup.SetupDbTx _db;

        [SetUp]
        public void SetUp()
        {
            _db = new ModuleTestSetup.SetupDbTx();
        }

        [TearDown]
        public void TearDown()
        {
            using (_db)
            { }
        }

        [Test]
        public async Task VerifyAgreement()
        {
            var agreement = Agreements.FindBlueprints;

            var bp1 = await new BlueprintBuilder().SaveAsync(_db.NhRepository);
            var bp2 = await new BlueprintBuilder().SaveAsync(_db.NhRepository);

            var blueprints = await agreement.Executable().ExecuteAsync();

            blueprints.Should().BeEquivalentTo(agreement.Result().Mutate(r =>
            {
                r[0].With(bp => bp.Id, bp1.Id);
                r[1].With(bp => bp.Id, bp2.Id);
            }));
        }

        [Test]
        public async Task OrdersByName()
        {
            await new BlueprintBuilder().With(bp => bp.Name, "Bp3").SaveAsync(_db.NhRepository);
            await new BlueprintBuilder().With(bp => bp.Name, "Bp1").SaveAsync(_db.NhRepository);
            await new BlueprintBuilder().With(bp => bp.Name, "Bp2").SaveAsync(_db.NhRepository);

            var defaultUserId = new BlueprintBuilder().Value().OwnedByUserId;

            var blueprints = await new FindBlueprintsQuery { UserId = defaultUserId }.ExecuteAsync();

            blueprints.Select(bp => bp.Name).Should().Equal("Bp1", "Bp2", "Bp3");
        }

        [Test]
        public async Task FiltersUserId()
        {
            var bp1 = await new BlueprintBuilder().With(bp => bp.OwnedByUserId, 123).SaveAsync(_db.NhRepository);
            var bp2 = await new BlueprintBuilder().With(bp => bp.OwnedByUserId, 234).SaveAsync(_db.NhRepository);

            var blueprints = await new FindBlueprintsQuery { UserId = 234 }.ExecuteAsync();

            blueprints.Select(bp => bp.Id).Should().Equal(bp2.Id);
        }
    }
}
