using System.Diagnostics;
using Lucid.Persistence;

namespace Demo.Domain.Utility
{
    public class DemoEntity : Entity<int>
    {
        protected static IDemoRepository Repository {[DebuggerStepThrough] get { return Registry.Repository; } }
    }
}
