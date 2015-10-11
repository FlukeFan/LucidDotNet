using System.ComponentModel.DataAnnotations;
using Demo.Domain.Orgs.Responses;
using Lucid.Domain.Remote;

namespace Demo.Domain.Orgs.Commands
{
    public class Login : Command<UserDto>
    {
        [Required(ErrorMessage = "Please supply an Email")]
        public string Email { get; set; }

        public override UserDto Execute()
        {
            var user = User.Login(Email);
            return new UserDto(user);
        }
    }
}
