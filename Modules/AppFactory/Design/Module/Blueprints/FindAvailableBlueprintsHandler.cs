using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lucid.Infrastructure.Lib.Facade;
using Lucid.Modules.AppFactory.Design.Contract;

namespace Lucid.Modules.AppFactory.Design.Blueprints
{
    public class FindAvailableBlueprintsHandler : IHandleQueryAsync<FindAvailableBlueprints, List<BlueprintDto>>
    {
        public Task<List<BlueprintDto>> Find(FindAvailableBlueprints query)
        {
            return Task.FromResult(
                Registry.Repository.Value.Query<Blueprint>()
                    .Select(bp => bp.ToDto())
                    .ToList());
        }
    }
}
