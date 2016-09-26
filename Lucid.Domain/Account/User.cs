using System;
using Lucid.Domain.Utility;

namespace Lucid.Domain.Account
{
    public class User : LucidEntity
    {
        protected User() { }

        public virtual string   Email           { get; protected set; }
        public virtual DateTime LastLoggedIn    { get; protected set; }

        public static User Login(string email)
        {
            var user =
                Repository.Query<User>()
                    .Filter(u => u.Email == email)
                    .SingleOrDefault();

            if (user == null)
            {
                user = new User
                {
                    Email = email,
                };
            }

            user.LastLoggedIn = Registry.NowUtc();
            return Repository.Save(user);
        }
    }
}
