using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Lucid.Infrastructure.Lib.Facade;
using Lucid.Infrastructure.Lib.Facade.Exceptions;

namespace Lucid.Modules.AppFactory.Design.Blueprints
{
    public class StartCommand : CommandAsync<Blueprint>
    {
        [Required(ErrorMessage = "Please supply a Name")]
        public string Name { get; set; }

        public override Task<Blueprint> ExecuteAsync()
        {
            throw new FacadeException("Not implemented yet ...");
        }
    }
}
