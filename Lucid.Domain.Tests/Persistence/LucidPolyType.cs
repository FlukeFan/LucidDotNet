using System;

namespace Lucid.Domain.Tests.Persistence
{
    public class LucidPolyType : LucidEntity
    {
        protected LucidPolyType() { }

        public virtual string   Email           { get; protected set; }
        public virtual DateTime LastLoggedIn    { get; protected set; }
    }
}
