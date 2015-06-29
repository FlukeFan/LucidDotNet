using System;

namespace Lucid.Domain.Utility
{
    public class DomainContext
    {
        public static Func<DateTime> NowUtc = () => DateTime.UtcNow;
    }
}
