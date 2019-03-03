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
            VerifyNameIsUnique(cmd.OwnedByUserId, cmd.Name);

            return await Repository.SaveAsync(
                new Blueprint
                {
                    OwnedByUserId = cmd.OwnedByUserId,
                    Name = cmd.Name,
                });
        }

        private static void VerifyNameIsUnique(int ownedByUserId, string name)
        {
            var existingBlueprint = Repository.Query<Blueprint>()
                .Where(bp => bp.OwnedByUserId == ownedByUserId)
                .Where(bp => bp.Name == name)
                .SingleOrDefault();

            if (existingBlueprint != null)
                throw new FacadeException($"There is already a Blueprint with the name '{existingBlueprint.Name}'");
        }
    }
}
