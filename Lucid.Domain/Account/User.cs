using System;
using Lucid.Domain.Utility;

namespace Lucid.Domain.Account
{
    public class User : LucidEntity
    {
        protected User() { }

        public virtual string   Name            { get; protected set; }
        public virtual DateTime LastLoggedIn    { get; protected set; }

        public static User Login(string name)
        {
            var existingUser =
                Repository.Query<User>()
                    .Filter(u => u.Name == name)
                    .SingleOrDefault();

            var user = existingUser ??
                new User
                {
                    Name = name,
                };

            user.LastLoggedIn = Registry.NowUtc();

            if (existingUser == null)
                Repository.Save(user);

            return user;
        }
    }
}
