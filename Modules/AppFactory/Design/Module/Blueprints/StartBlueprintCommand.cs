using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Lucid.Infrastructure.Lib.Facade;

namespace Lucid.Modules.AppFactory.Design.Blueprints
{
    public class StartBlueprintCommand : CommandAsync<Blueprint>
    {
        [Required(ErrorMessage = "Please supply a Name")]
        public string Name { get; set; }

        public override Task<Blueprint> ExecuteAsync()
        {
            throw new NotImplementedException();
        }
    }
}
