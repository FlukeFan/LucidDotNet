using NHibernate;
using Reposify;
using Reposify.NHibernate;

namespace Lucid.Lib.Domain.SqlServer
{
    public interface INhSqlRepository :
        IRepositoryAsync,
        IDbExecutorAsync,
        ILinqQueryable,
        IDbLinqExecutorAsync
    { }

    public class NhSqlRepository : NhRepository, INhSqlRepository
    {
        public NhSqlRepository(ISession session) : base(session) { }
    }
}
