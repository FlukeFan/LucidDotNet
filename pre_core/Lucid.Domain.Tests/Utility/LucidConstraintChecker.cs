using Reposify.Testing;

namespace Lucid.Domain.Tests.Utility
{
    public class LucidConstraintChecker : ConstraintChecker
    {
        public LucidConstraintChecker() : base(isMsSql: true) { }
    }
}
