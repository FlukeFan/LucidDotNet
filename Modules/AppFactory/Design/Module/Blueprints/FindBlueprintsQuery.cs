using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lucid.Lib.Facade;
using Lucid.Modules.AppFactory.Design.Contract;
using Reposify;

namespace Lucid.Modules.AppFactory.Design.Blueprints
{
    public class FindBlueprintsQuery : QueryAsync<List<BlueprintDto>>, IDbLinq<Blueprint>
    {
        public int UserId;

        public override async Task<List<BlueprintDto>> ExecuteAsync()
        {
            var blueprints = await Registry.Repository.Value.ListAsync(this);

            return blueprints
                .Select(bp => bp.ToDto())
                .ToList();
        }

        public IQueryable<Blueprint> Prepare(IQueryable<Blueprint> queryable)
        {
            return queryable
                .Where(bp => bp.OwnedByUserId == UserId)
                .OrderBy(bp => bp.Name);
        }
    }
}
