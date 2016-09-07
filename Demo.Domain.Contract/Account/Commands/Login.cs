using System.ComponentModel.DataAnnotations;
using Demo.Domain.Contract.Account.Responses;
using Lucid.Facade.Execution;

namespace Demo.Domain.Contract.Account.Commands
{
    public class Login : ICommand<UserDto>
    {
        [Required(ErrorMessage = "Please supply an Email")]
        public string Email { get; set; }
    }
}
