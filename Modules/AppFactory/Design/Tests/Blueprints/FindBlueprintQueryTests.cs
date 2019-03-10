using System.Threading.Tasks;
using FluentAssertions;
using Lucid.Infrastructure.Lib.Testing.Execution;
using NHibernate;
using NUnit.Framework;

namespace Lucid.Modules.AppFactory.Design.Tests.Blueprints
{
    [TestFixture]
    public class FindBlueprintQueryTests : ModuleTestSetup.LogicTest
    {
        [Test]
        public async Task Agreement()
        {
            using (var db = new ModuleTestSetup.SetupDbTx())
            {
                var agreement = Agreements.FindBlueprint;
                var bp = await Agreements.Start.Executable().ExecuteAsync();

                db.Session.Clear();

                var result = await agreement.Executable().Mutate(c => c.BlueprintId = bp.Id).ExecuteAsync();

                NHibernateUtil.IsInitialized(result).Should().BeTrue();
                result.Should().BeEquivalentTo(agreement.Result().Mutate(r => r.With(e => e.Id, result.Id)));
            }
        }
    }
}
