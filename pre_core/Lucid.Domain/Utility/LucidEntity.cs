using System.Diagnostics;
using Reposify;

namespace Lucid.Domain.Utility
{
    public class LucidEntity : Entity<int>
    {
        protected static ILucidRepository Repository {[DebuggerStepThrough] get { return DomainRegistry.Repository; } }
    }
}
