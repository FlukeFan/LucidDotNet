using System;

namespace Lucid.Infrastructure.Lib.Domain
{
    public class DomainRegistry
    {
        public static Func<DateTime> UtcNow = () => DateTime.UtcNow;
    }
}
