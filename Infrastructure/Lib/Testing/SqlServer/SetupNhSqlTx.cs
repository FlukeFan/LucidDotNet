using System;
using Lucid.Infrastructure.Lib.Domain.SqlServer;
using NHibernate;

namespace Lucid.Infrastructure.Lib.Testing.SqlServer
{
    public class SetupNhSqlTx : IDisposable
    {
        public SetupNhSqlTx(ISessionFactory sessionFactory, DbState dbState)
        {
            dbState.Clean(() =>
            {
                using (var repo = new NhSqlRepository(sessionFactory.OpenSession()))
                {
                    repo.BeginTransaction();
                    Clean(repo);
                    repo.Commit();
                }
            });

            Session = sessionFactory.OpenSession();
            NhRepository = new NhSqlRepository(Session);
            NhRepository.BeginTransaction();
        }

        public ISession         Session         { get; private set; }
        public NhSqlRepository  NhRepository    { get; private set; }

        public virtual void Clean(NhSqlRepository repository) { }

        public virtual void Dispose()
        {
            using (NhRepository)
            using (Session)
            { }
        }
    }
}
