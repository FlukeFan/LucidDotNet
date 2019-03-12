using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lucid.Infrastructure.Lib.Facade;
using Lucid.Modules.AppFactory.Design.Contract;
using Reposify;

namespace Lucid.Modules.AppFactory.Design.Blueprints
{
    public class FindAvailableBlueprintsHandler : IHandleQueryAsync<FindAvailableBlueprints, List<BlueprintDto>>, IDbLinq<Blueprint>
    {
        public async Task<List<BlueprintDto>> Find(FindAvailableBlueprints query)
        {
            var blueprintList = await Registry.Repository.Value.ListAsync(this);
            return blueprintList.Select(bp => bp.ToDto()).ToList();
        }

        public IQueryable<Blueprint> Prepare(IQueryable<Blueprint> queryable)
        {
            return queryable;
        }
    }
}
