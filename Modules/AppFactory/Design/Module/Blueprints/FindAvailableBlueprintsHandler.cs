using System.Collections.Generic;
using System.Threading.Tasks;
using Lucid.Infrastructure.Lib.Facade;
using Lucid.Modules.AppFactory.Design.Contract;

namespace Lucid.Modules.AppFactory.Design.Blueprints
{
    public class FindAvailableBlueprintsHandler : IHandleQueryAsync<FindAvailableBlueprints, List<BlueprintDto>>
    {
        public async Task<List<BlueprintDto>> Find(FindAvailableBlueprints query)
        {
            return await new FindBlueprintsQuery { UserId = query.UserId }.ExecuteAsync();
        }
    }
}
