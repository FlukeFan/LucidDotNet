using Reposify.Testing;

namespace Lucid.Domain.Tests.Utility
{
    public class LucidConsistencyInspector : ConsistencyInspector
    {
        public LucidConsistencyInspector() : base(isMsSql: true) { }
    }
}
