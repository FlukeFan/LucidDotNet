using System;

namespace Lucid.Lib.Domain
{
    public class DomainRegistry
    {
        public static Func<DateTime> UtcNow = () => DateTime.UtcNow;
    }
}
