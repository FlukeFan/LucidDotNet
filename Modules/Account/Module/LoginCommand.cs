using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Lucid.Infrastructure.Lib.Facade;

namespace Lucid.Modules.Account
{
    public class LoginCommand : CommandAsync<User>
    {
        [Required(ErrorMessage = "Please supply a User Name")]
        public string UserName { get; set; }

        public override async Task<User> ExecuteAsync()
        {
            return await User.Login(this);
        }
    }
}
