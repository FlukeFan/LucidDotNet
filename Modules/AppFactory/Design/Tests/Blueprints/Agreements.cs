using System.Collections.Generic;
using Lucid.Lib.Facade.Pledge;
using Lucid.Modules.AppFactory.Design.Blueprints;
using Lucid.Modules.AppFactory.Design.Contract;

namespace Lucid.Modules.AppFactory.Design.Tests.Blueprints
{
    public class Agreements
    {
        public static IAgreement<FindBlueprintsQuery, List<BlueprintDto>> FindBlueprints =
            AgreementBuilder.For(() =>
                new FindBlueprintsQuery
                {
                    UserId = Defaults.UserId,
                })
            .Result(() =>
                new List<BlueprintDto>
                {
                    new BlueprintDtoDefault { Id = 101 },
                    new BlueprintDtoDefault { Id = 102 },
                });

        public static IAgreement<FindBlueprintQuery, BlueprintDto> FindBlueprint =
            AgreementBuilder.For(() =>
                new FindBlueprintQuery
                {
                    BlueprintId = 123,
                })
            .Result(() => new BlueprintDtoDefault { Id = 123 });

        public static IAgreement<StartEditCommand, int> Start =
            AgreementBuilder.For(() =>
                new StartEditCommand
                {
                    OwnedByUserId = Defaults.UserId,
                    Name = "TestBlueprint",
                })
            .Result(() => 101);

        public static IAgreement<StartEditCommand, int> Edit =
            AgreementBuilder.For(() =>
                new StartEditCommand
                {
                    BlueprintId = 123,
                    OwnedByUserId = Defaults.UserId,
                    Name = "UpdatedBlueprint",
                })
            .Result(() => 101);
    }
}
