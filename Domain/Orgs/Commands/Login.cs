using Lucid.Domain.Orgs.Responses;
using Lucid.Domain.Utility;

namespace Lucid.Domain.Orgs.Commands
{
    public class Login : Command<UserDto>
    {
        public string Email;

        public override UserDto Execute()
        {
            var user = User.Login(Email);
            return new UserDto(user);
        }
    }
}
