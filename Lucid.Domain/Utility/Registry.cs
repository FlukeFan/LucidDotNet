﻿using System;

namespace Lucid.Domain.Utility
{
    public class Registry
    {
        public static Func<DateTime> NowUtc = () => DateTime.UtcNow;

        [ThreadStatic]
        public static ILucidRepository Repository;
    }
}