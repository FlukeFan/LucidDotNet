﻿using System;

namespace Lucid.Domain.Tests.Persistence
{
    public class LucidPolyType : LucidEntity
    {
        public enum Values
        {
            Val1 = 1,
            Val2 = 2,
            Val3 = 3,
        };

        protected LucidPolyType() { }

        public virtual string       String              { get; protected set; }
        public virtual int          Int                 { get; protected set; }
        public virtual DateTime     DateTime            { get; protected set; }
        public virtual Values       Enum                { get; protected set; }
        public virtual int?         NullableInt         { get; protected set; }
        public virtual DateTime?    NullableDateTime    { get; protected set; }
        public virtual Values?      NullableEnum        { get; protected set; }
    }
}
