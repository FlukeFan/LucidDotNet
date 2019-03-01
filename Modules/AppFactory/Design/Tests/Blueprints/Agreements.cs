using System.Collections.Generic;
using Lucid.Infrastructure.Lib.Testing.Execution;
using Lucid.Modules.AppFactory.Design.Blueprints;

namespace Lucid.Modules.AppFactory.Design.Tests.Blueprints
{
    public class Agreements
    {
        public static Agreement<FindBlueprintsQuery, List<Blueprint>> FindBlueprints =
            AgreementBuilder.For(() =>
                new FindBlueprintsQuery
                {
                    UserId = Defaults.UserId,
                })
            .Result(() =>
                new List<Blueprint>
                {
                    new BlueprintBuilder().Value(),
                    new BlueprintBuilder().Value(),
                });

        public static Agreement<StartCommand, Blueprint> Start =
            AgreementBuilder.For(() =>
                new StartCommand
                {
                    OwnedByUserId = Defaults.UserId,
                    Name = "TestBlueprint",
                })
            .Result(() =>
                new BlueprintBuilder()
                    .Value());
    }
}
