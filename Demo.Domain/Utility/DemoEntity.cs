using System.Diagnostics;
using Reposify;

namespace Demo.Domain.Utility
{
    public class DemoEntity : Entity<int>
    {
        protected static IDemoRepository Repository {[DebuggerStepThrough] get { return Registry.Repository; } }
    }
}
