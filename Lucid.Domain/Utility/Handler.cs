using System.Diagnostics;

namespace Lucid.Domain.Utility
{
    public class Handler
    {
        protected static ILucidRepository Repository { [DebuggerStepThrough] get { return DomainRegistry.Repository; } }
    }
}
