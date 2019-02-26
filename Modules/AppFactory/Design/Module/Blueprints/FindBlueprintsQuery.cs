using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lucid.Infrastructure.Lib.Facade;
using NHibernate.Linq;

namespace Lucid.Modules.AppFactory.Design.Blueprints
{
    public class FindBlueprintsQuery : QueryAsync<List<Blueprint>>
    {
        public override async Task<List<Blueprint>> ExecuteAsync()
        {
            return await Registry.Repository.Value.Query<Blueprint>()
                .OrderBy(bp => bp.Name)
                .ToListAsync();
        }
    }
}
