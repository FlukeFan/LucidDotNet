using Lucid.Domain.Account.Responses;
using Lucid.Domain.Contract.Account.Commands;
using Lucid.Domain.Contract.Account.Responses;
using SimpleFacade;

namespace Lucid.Domain.Account.Commands
{
    public class LoginHandler : IHandleCommand<Login, UserDto>
    {
        public UserDto Execute(Login cmd)
        {
            var user = User.Login(cmd.Email);
            return UserDtoMapper.Map(user);
        }
    }
}
