﻿namespace Lucid.Domain.Persistence
{
    public abstract class Entity<TId> : IEntity<TId>
    {
        public virtual TId Id { get; protected set; }
    }
}
