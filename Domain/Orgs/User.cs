using System;
using Lucid.Domain.Utility;
using Lucid.Domain.Utility.Queries;

namespace Lucid.Domain.Orgs
{
    public class User : Entity
    {
        protected User() { }

        public virtual string   Email           { get; protected set; }
        public virtual DateTime LastLoggedIn    { get; protected set; }

        public static User Login(string email)
        {
            var user =
                Registry.Repository.Query<User>()
                    .Filter(Where<User>.PropEq(u => u.Email, email))
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
