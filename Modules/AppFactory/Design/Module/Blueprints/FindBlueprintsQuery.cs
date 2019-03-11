using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lucid.Infrastructure.Lib.Facade;
using Lucid.Modules.AppFactory.Design.Contract;
using NHibernate.Linq;

namespace Lucid.Modules.AppFactory.Design.Blueprints
{
    public class FindBlueprintsQuery : QueryAsync<List<BlueprintDto>>
    {
        public int UserId;

        public override async Task<List<BlueprintDto>> ExecuteAsync()
        {
            return await Registry.Repository.Value.Query<Blueprint>()
                .Where(bp => bp.OwnedByUserId == UserId)
                .OrderBy(bp => bp.Name)
                .Select(bp => bp.ToDto())
                .ToListAsync();
        }
    }
}
