using System;

namespace Lucid.Domain.Utility
{
    public class DomainRegistry
    {
        public static Func<DateTime> NowUtc = () => DateTime.UtcNow;

        [ThreadStatic]
        public static ILucidRepository Repository;
    }
}
