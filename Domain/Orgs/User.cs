
namespace Lucid.Domain.Orgs
{
    public class User
    {
        public string Email { get; protected set; }

        public static User Login(string email)
        {
            var user = new User
            {
                Email = email,
            };

            return user;
        }
    }
}
