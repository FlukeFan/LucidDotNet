using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Lucid.Infrastructure.Lib.Facade;

namespace Lucid.Modules.Account
{
    public class Login : CommandAsync<int>
    {
        [Required(ErrorMessage = "Please supply a User Name")]
        public string UserName { get; set; }

        public override Task<int> ExecuteAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}
