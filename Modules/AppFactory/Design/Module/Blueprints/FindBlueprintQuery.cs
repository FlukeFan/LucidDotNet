using System.Threading.Tasks;
using Lucid.Infrastructure.Lib.Facade;
using NHibernate;

namespace Lucid.Modules.AppFactory.Design.Blueprints
{
    public class FindBlueprintQuery : QueryAsync<Blueprint>
    {
        public int BlueprintId;

        public override async Task<Blueprint> ExecuteAsync()
        {
            var blueprint = await Registry.Repository.Value.LoadAsync<Blueprint>(BlueprintId);
            NHibernateUtil.Initialize(blueprint);
            return blueprint;
        }
    }
}
