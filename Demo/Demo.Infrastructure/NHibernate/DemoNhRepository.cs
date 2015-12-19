using System;
using Demo.Domain.Utility;
using Lucid.Persistence.NHibernate;

namespace Demo.Infrastructure.NHibernate
{
    public class DemoNhRepository : NhRepository<int>, IDemoRepository
    {
        public static void Init(string connection, Type type)
        {
            var sessionFactory = NhHelper.CreateSessionFactory<int>(connection, type);
            Init(sessionFactory);
        }
    }
}
