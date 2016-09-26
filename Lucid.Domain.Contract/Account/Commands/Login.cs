using System.ComponentModel.DataAnnotations;
using Lucid.Domain.Contract.Account.Responses;
using SimpleFacade;

namespace Lucid.Domain.Contract.Account.Commands
{
    public class Login : ICommand<UserDto>
    {
        [Required(ErrorMessage = "Please supply an Email")]
        public string Email { get; set; }
    }
}
