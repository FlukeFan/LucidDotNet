using System.Linq;
using System.Threading.Tasks;
using Lucid.Infrastructure.Lib.Facade.Exceptions;

namespace Lucid.Modules.AppFactory.Design.Blueprints
{
    public class Blueprint : Registry.Entity
    {
        protected Blueprint() { }

        public virtual int      OwnedByUserId   { get; protected set; }
        public virtual string   Name            { get; protected set; }

        public static async Task<Blueprint> Start(StartCommand cmd)
        {
            var existingBlueprint = Repository.Query<Blueprint>()
                .Where(bp => bp.OwnedByUserId == cmd.OwnedByUserId)
                .Where(bp => bp.Name == cmd.Name)
                .SingleOrDefault();

            if (existingBlueprint != null)
                throw new FacadeException($"There is already a Blueprint with the name '{cmd.Name}'");

            return await Repository.SaveAsync(
                new Blueprint
                {
                    OwnedByUserId = cmd.OwnedByUserId,
                    Name = cmd.Name,
                });
        }
    }
}
