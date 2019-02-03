using NHibernate;
using Reposify;
using Reposify.NHibernate;

namespace Lucid.Infrastructure.Lib.Domain.SqlServer
{
    public interface INhSqlRepository :
        IRepositoryAsync,
        IDbExecutorAsync,
        ILinqQueryable,
        IDbLinqExecutor
    { }

    public class NhSqlRepository : NhRepository, INhSqlRepository
    {
        public NhSqlRepository(ISession session) : base(session) { }
    }
}
