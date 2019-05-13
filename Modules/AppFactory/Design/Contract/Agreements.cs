using System.Collections.Generic;
using Lucid.Lib.Facade.Pledge;

namespace Lucid.Modules.AppFactory.Design.Contract
{
    public class Agreements
    {
        public static IAgreement<FindAvailableBlueprints, List<BlueprintDto>> FindAvailableBlueprints =
            AgreementBuilder.For(() =>
                new FindAvailableBlueprints
                {
                    UserId = Defaults.UserId,
                })
            .Result(() =>
                new List<BlueprintDto>
                {
                    new BlueprintDto { Id = 101, Name = "Blueprint1" },
                    new BlueprintDto { Id = 102, Name = "Blueprint2" },
                });
    }
}

