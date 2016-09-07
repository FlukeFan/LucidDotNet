using Reposify.Testing;

namespace Demo.Domain.Tests.Utility
{
    public class DemoConsistencyInspector : ConsistencyInspector
    {
        public DemoConsistencyInspector() : base(isMsSql: true) { }
    }
}
