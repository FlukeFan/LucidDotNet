using System;
using Lucid.Infrastructure.Lib.Domain.SqlServer;
using NHibernate;

namespace Lucid.Infrastructure.Lib.Testing.SqlServer
{
    public class SetupNhSqlTx : IDisposable
    {
        public SetupNhSqlTx(ISessionFactory sessionFactory)
        {
            Session = sessionFactory.OpenSession();
            NhRepository = new NhSqlRepository(Session);
            NhRepository.BeginTransaction();
        }

        public ISession         Session         { get; private set; }
        public NhSqlRepository  NhRepository    { get; private set; }

        public virtual void Dispose()
        {
            using (NhRepository)
            using (Session)
            { }
        }
    }
}
