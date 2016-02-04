using Demo.Domain.Account.Responses;
using Demo.Domain.Contract.Account.Commands;
using Demo.Domain.Contract.Account.Responses;
using Lucid.Facade.Execution;

namespace Demo.Domain.Account.Commands
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
