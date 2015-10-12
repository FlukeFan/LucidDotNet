using System;
using Lucid.Domain.Persistence;

namespace Lucid.Domain.Tests.Persistence
{
    public class LucidEntity : Entity<int>
    {
        protected LucidEntity() { }

        public virtual string   Email           { get; protected set; }
        public virtual DateTime LastLoggedIn    { get; protected set; }
    }
}
