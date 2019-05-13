using System.Threading.Tasks;
using Lucid.Lib.Facade;
using Lucid.Modules.AppFactory.Design.Contract;

namespace Lucid.Modules.AppFactory.Design.Blueprints
{
    public class FindBlueprintQuery : QueryAsync<BlueprintDto>
    {
        public int BlueprintId;

        public override async Task<BlueprintDto> ExecuteAsync()
        {
            var blueprint = await Registry.Repository.Value.LoadAsync<Blueprint>(BlueprintId);
            return blueprint.ToDto();
        }
    }
}
