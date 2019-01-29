using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Lucid.Infrastructure.Lib.Facade;

namespace Lucid.Modules.Account
{
    public class Login : Command<int>
    {
        [Required(ErrorMessage = "Please supply a User Name")]
        public string UserName { get; set; }

        public override Task<int> Execute()
        {
            throw new System.NotImplementedException();
        }
    }
}
