﻿namespace Lucid.Modules.AppFactory.Design.Blueprints
{
    public class Blueprint : Registry.Entity
    {
        protected Blueprint() { }

        public virtual int      OwnedByUserId   { get; protected set; }
        public virtual string   Name            { get; protected set; }
    }
}