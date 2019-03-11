using System.Threading.Tasks;
using FluentAssertions;
using Lucid.Infrastructure.Lib.Testing.Execution;
using NUnit.Framework;

namespace Lucid.Modules.AppFactory.Design.Tests.Blueprints
{
    [TestFixture]
    public class FindBlueprintQueryTests : ModuleTestSetup.LogicTest
    {
        [Test]
        public async Task Agreement()
        {
            var agreement = Agreements.FindBlueprint;

            using (var db = new ModuleTestSetup.SetupDbTx())
            {
                var bpId = await Agreements.Start.Executable().ExecuteAsync();

                var result = await agreement.Executable().Mutate(c => c.BlueprintId = bpId).ExecuteAsync();

                result.Should().BeEquivalentTo(agreement.Result().Mutate(r => r.Id = result.Id));
            }
        }
    }
}
