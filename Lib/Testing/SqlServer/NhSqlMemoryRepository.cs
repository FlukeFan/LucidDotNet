using Lucid.Lib.Domain.SqlServer;
using Reposify.Testing;

namespace Lucid.Lib.Testing.SqlServer
{
    public class NhSqlMemoryRepository : MemoryRepository, INhSqlRepository
    {
        public NhSqlMemoryRepository(ConstraintChecker constraintChecker) : base(constraintChecker) { }
    }
}
