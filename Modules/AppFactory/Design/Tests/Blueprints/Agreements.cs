﻿using System.Collections.Generic;
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
                    new BlueprintBuilder().With(bp => bp.Id, 101).Value(),
                    new BlueprintBuilder().With(bp => bp.Id, 102).Value(),
                });

        public static Agreement<FindBlueprintQuery, Blueprint> FindBlueprint =
            AgreementBuilder.For(() =>
                new FindBlueprintQuery
                {
                    BlueprintId = 123,
                })
            .Result(() => new BlueprintBuilder().With(bp => bp.Id, 123).Value());

        public static Agreement<StartEditCommand, Blueprint> Start =
            AgreementBuilder.For(() =>
                new StartEditCommand
                {
                    OwnedByUserId = Defaults.UserId,
                    Name = "TestBlueprint",
                })
            .Result(() =>
                new BlueprintBuilder()
                    .Value());

        public static Agreement<StartEditCommand, Blueprint> Edit =
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
