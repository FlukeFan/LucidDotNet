using System;
using System.Linq;
using System.Threading.Tasks;

namespace Lucid.Modules.Security
{
    public class User : Registry.Entity
    {
        protected User() { }

        public virtual string   Name            { get; protected set; }
        public virtual DateTime LastLoggedIn    { get; protected set; }

        public async static Task<User> Login(LoginCommand cmd)
        {
            var existingUser = Repository.Query<User>()
                .Where(u => u.Name == cmd.UserName)
                .SingleOrDefault();

            if (existingUser == null)
            {
                var user = new User
                {
                    Name = cmd.UserName,
                };
                
                user.Login();
                return await Repository.SaveAsync(user);
            }
            else
            {
                existingUser.Login();
                return existingUser;
            }
        }

        protected virtual void Login()
        {
            LastLoggedIn = Registry.UtcNow();
        }
    }
}
