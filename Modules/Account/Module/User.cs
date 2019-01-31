using System;

namespace Lucid.Modules.Account
{
    public class User : Registry.Entity
    {
        protected User() { }

        public virtual string   Name            { get; protected set; }
        public virtual DateTime LastLoggedIn    { get; protected set; }
    }
}
