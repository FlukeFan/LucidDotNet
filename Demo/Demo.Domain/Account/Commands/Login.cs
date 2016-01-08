using System.ComponentModel.DataAnnotations;
using Demo.Domain.Account.Responses;
using Lucid.Domain.Execution;

namespace Demo.Domain.Account.Commands
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
