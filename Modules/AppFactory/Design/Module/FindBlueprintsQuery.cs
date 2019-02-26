using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lucid.Infrastructure.Lib.Facade;

namespace Lucid.Modules.AppFactory.Design
{
    public class FindBlueprintsQuery : QueryAsync<List<Blueprint>>
    {
        public override Task<List<Blueprint>> ExecuteAsync()
        {
            var blueprints = Registry.Repository.Value.Query<Blueprint>()
                .OrderBy(bp => bp.Name)
                .ToList();

            return Task.FromResult(blueprints);
        }
    }
}
