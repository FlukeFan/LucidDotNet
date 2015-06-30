using System;
using Lucid.Domain.Utility;

namespace Lucid.Domain.Orgs
{
    public class User : Entity
    {
        public string   Email           { get; protected set; }
        public DateTime LastLoggedIn    { get; protected set; }

        public static User Login(string email)
        {
            var user = new User
            {
                Email = email,
                LastLoggedIn = Registry.NowUtc(),
            };

            return Registry.Repository.Save(user);
        }
    }
}
