using System;
using NHibernate;
using Reposify.NHibernate;

namespace Lucid.Infrastructure.Lib.Testing.SqlServer
{
    public class NhSqlTxTest : IDisposable
    {
        public NhSqlTxTest(ISessionFactory sessionFactory)
        {
            Session = sessionFactory.OpenSession();
            NhRepository = new NhRepository(Session);
            NhRepository.BeginTransaction();
        }

        public ISession     Session         { get; private set; }
        public NhRepository NhRepository    { get; private set; }

        public void Dispose()
        {
            using (NhRepository)
            using (Session)
            { }
        }
    }
}
