using System;
using System.Threading.Tasks;

namespace Lucid.Modules.Account
{
    public class User : Registry.Entity
    {
        protected User() { }

        public virtual string   Name            { get; protected set; }
        public virtual DateTime LastLoggedIn    { get; protected set; }

        public async static Task<User> Login(Login cmd)
        {
            return await Repository.SaveAsync(new User
            {
                Name            = cmd.UserName,
                LastLoggedIn    = DateTime.UtcNow,
            });
        }
    }
}
