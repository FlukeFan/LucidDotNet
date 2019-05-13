using System;
using Lucid.Lib.Domain;

namespace Lucid.Lib.Testing.Domain
{
    public class SetupDomainRegistry : IDisposable
    {
        private Func<DateTime> _previousNow;

        public SetupDomainRegistry()
        {
            _previousNow = DomainRegistry.UtcNow;
        }

        public virtual void Dispose()
        {
            DomainRegistry.UtcNow = _previousNow;
        }
    }
}
