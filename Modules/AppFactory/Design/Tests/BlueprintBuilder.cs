using FluentAssertions;
using Reposify.Testing;

namespace Lucid.Modules.AppFactory.Design.Tests
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
            With(e => e.OwnedByUserId, 123);
            With(e => e.Name, "TestBlueprint");
        }
    }
}
