using Lucid.Infrastructure.Lib.Domain.SqlServer;
using Reposify.Testing;

namespace Lucid.Infrastructure.Lib.Testing.SqlServer
{
    public class NhSqlMemoryRepository : MemoryRepository, INhSqlRepository
    {
        public NhSqlMemoryRepository(ConstraintChecker constraintChecker) : base(constraintChecker) { }
    }
}
