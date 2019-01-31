using System;

namespace Lucid.Modules.Account
{
    public class User : Registry.Entity
    {
        protected User() { }

        public string   Name            { get; protected set; }
        public DateTime LastLoggedOut   { get; protected set; } // deliberate wrong name until we verify persistence fails
    }
}
