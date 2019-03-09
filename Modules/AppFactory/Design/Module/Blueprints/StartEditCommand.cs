using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Lucid.Infrastructure.Lib.Facade;
using Lucid.Infrastructure.Lib.Facade.Exceptions;

namespace Lucid.Modules.AppFactory.Design.Blueprints
{
    public class StartEditCommand : CommandAsync<Blueprint>
    {
        public int BlueprintId      { get; set; }
        public int OwnedByUserId    { get; set; }

        [Required(ErrorMessage = "Please supply a Name")]
        public string Name { get; set; }

        public override async Task<Blueprint> ExecuteAsync()
        {
            if (OwnedByUserId == 0)
                throw new FacadeException("User id not specified");

            return (BlueprintId == 0)
                ? await Blueprint.StartAsync(this)
                : await Blueprint.EditAsync(this);
        }
    }
}
