using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Lucid.Lib.Facade;

namespace Lucid.Modules.Security
{
    public class LoginCommand : CommandAsync<User>
    {
        [Required(ErrorMessage = "Please supply a User Name")]
        [MaxLength(255, ErrorMessage = "Please supply a User Name less than 255 characters")]
        public string UserName { get; set; }

        public override async Task<User> ExecuteAsync()
        {
            return await User.Login(this);
        }
    }
}
