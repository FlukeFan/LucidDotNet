using FluentAssertions;
using Lucid.Infrastructure.Lib.Facade.Pledge;
using Lucid.Modules.AppFactory.Design.Blueprints;
using Lucid.Modules.AppFactory.Design.Contract;
using Reposify.Testing;

namespace Lucid.Modules.AppFactory.Design.Tests.Blueprints
{
    public class BlueprintDtoDefault : BlueprintDto
    {
        public BlueprintDtoDefault()
        {
            Name = "TestBlueprint";
        }
    }

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
