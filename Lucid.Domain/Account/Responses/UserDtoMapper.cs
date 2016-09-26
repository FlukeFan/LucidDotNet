using Lucid.Domain.Contract.Account.Responses;

namespace Lucid.Domain.Account.Responses
{
    public static class UserDtoMapper
    {
        public static UserDto Map(User user)
        {
            return new UserDto
            {
                UserId  = user.Id,
                Email   = user.Email,
            };
        }
    }
}
