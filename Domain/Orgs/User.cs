using System;
using Lucid.Domain.Utility;

namespace Lucid.Domain.Orgs
{
    public class User : Entity
    {
        protected User() { }

        public string   Email           { get; protected set; }
        public DateTime LastLoggedIn    { get; protected set; }

        public static User Login(string email)
        {
            var user =
                Registry.Repository.Query<User>()
                    .SingleOrDefault();

            if (user == null)
            {
                user = new User
                {
                    Email = email,
                };
            }

            user.LastLoggedIn = Registry.NowUtc();
            return Registry.Repository.Save(user);
        }
    }
}
