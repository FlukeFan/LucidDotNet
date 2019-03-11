using System.Collections.Generic;
using Lucid.Infrastructure.Lib.Testing.Execution;
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

        public static IAgreement<StartEditCommand, Blueprint> Start =
            AgreementBuilder.For(() =>
                new StartEditCommand
                {
                    OwnedByUserId = Defaults.UserId,
                    Name = "TestBlueprint",
                })
            .Result(() =>
                new BlueprintBuilder()
                    .Value());

        public static IAgreement<StartEditCommand, Blueprint> Edit =
            AgreementBuilder.For(() =>
                new StartEditCommand
                {
                    BlueprintId = 123,
                    OwnedByUserId = Defaults.UserId,
                    Name = "UpdatedBlueprint",
                })
            .Result(() =>
                new BlueprintBuilder()
                    .With(bp => bp.Name, "UpdatedBlueprint")
                    .Value());
    }
}
