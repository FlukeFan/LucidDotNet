using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Lucid.Lib.MvcApp
{
    public static class UserExtensions
    {
        public const string UserItemKey = "LoggedInUser";

        public static User LoggedInUser(this HttpContext context)
        {
            if (!context.User.Identity.IsAuthenticated)
                return null;

            if (context.Items[UserItemKey] == null)
            {
                var claims = context.User.Claims.ToDictionary(c => c.Type, c => c.Value);

                var user = new User
                {
                    Id = int.Parse(claims["Id"]),
                    Name = claims[ClaimTypes.Name],
                };

                context.Items.Add(UserItemKey, user);
            }

            return (User)context.Items[UserItemKey];
        }
    }
}
