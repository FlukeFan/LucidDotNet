using System.Threading.Tasks;
using Lucid.Infrastructure.Lib.Facade;

namespace Lucid.Modules.AppFactory.Design.Blueprints
{
    public class FindBlueprintQuery : QueryAsync<Blueprint>
    {
        public int BlueprintId;

        public override async Task<Blueprint> ExecuteAsync()
        {
            return await Registry.Repository.Value.LoadAsync<Blueprint>(0); // broken until we add test
        }
    }
}
