using FluentAssertions;
using Lucid.Infrastructure.Lib.Testing.Execution;
using Lucid.Modules.AppFactory.Design.Blueprints;
using Reposify.Testing;

namespace Lucid.Modules.AppFactory.Design.Tests.Blueprints
{
    public class BlueprintBuilder : Builder<Blueprint>
    {
        static BlueprintBuilder()
        {
            CustomChecks.Add<Blueprint>((cc, e) =>
            {
                cc.Check(() => e.OwnedByUserId, uid => uid.Should().NotBe(0));
                cc.CheckNotNull(() => e.Name);
            });
        }

        public BlueprintBuilder()
        {
            With(e => e.OwnedByUserId, Defaults.UserId);
            With(e => e.Name, "TestBlueprint");
        }
    }
}
