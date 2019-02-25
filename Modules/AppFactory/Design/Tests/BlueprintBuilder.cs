using Reposify.Testing;

namespace Lucid.Modules.AppFactory.Design.Tests
{
    public class BlueprintBuilder : Builder<Blueprint>
    {
        static BlueprintBuilder()
        {
            //CustomChecks.Add<Blueprint>((cc, bp) =>
            //{
            //    cc.Check(() => bp.OwnedByUserId, uid => uid.Should().NotBe(0));
            //    cc.CheckNotNull(() => bp.Name);
            //});
        }

        public BlueprintBuilder()
        {
            With(u => u.OwnedByUserId, 123);
            With(u => u.Name, "TestBlueprint");
        }
    }
}
