
namespace Demo.Domain.Account.Responses
{
    public class UserDto
    {
        public int      UserId;
        public string   Email;

        public UserDto() { }

        public UserDto(User user)
        {
            UserId = user.Id;
            Email = user.Email;
        }
    }
}
