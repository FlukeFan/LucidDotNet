using System.ComponentModel.DataAnnotations;
using Lucid.Domain.Contract.Account.Responses;
using SimpleFacade;

namespace Lucid.Domain.Contract.Account.Commands
{
    public class Login : ICommand<UserDto>
    {
        [Required(ErrorMessage = "Please supply a Name")]
        [MinLength(4, ErrorMessage = "Please supply a Name of at least 4 characters")]
        public string Name { get; set; }
    }
}
