using System;

namespace Demo.Domain.Utility
{
    public class Registry
    {
        public static Func<DateTime> NowUtc = () => DateTime.UtcNow;

        [ThreadStatic]
        public static IDemoRepository Repository;
    }
}
