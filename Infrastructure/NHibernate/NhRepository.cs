using System;
using System.Collections.Generic;
using Lucid.Domain.Utility;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Connection;
using NHibernate.Dialect;
using NHibernate.Driver;

namespace Lucid.Infrastructure.NHibernate
{
    public class NhRepository : IRepository, IDisposable
    {
        public static ISessionFactory SessionFactory { get; protected set; }

        public static void Init(string connectionString)
        {
            var config = new Configuration();

            config.DataBaseIntegration(db =>
            {
                db.ConnectionString = connectionString;
                db.ConnectionProvider<DriverConnectionProvider>();
                db.Driver<SqlClientDriver>();
                db.Dialect<MsSql2008Dialect>();
                db.KeywordsAutoImport = Hbm2DDLKeyWords.AutoQuote;
            });

            var mappings = Mapping.CreateMappings();
            config.AddDeserializedMapping(mappings, "DomainMapping");

            SessionFactory = config.BuildSessionFactory();
        }

        private ISession        _session;
        private ITransaction    _transaction;

        public NhRepository()
        {
            if (SessionFactory == null)
                throw new Exception("Call Init() once to setup the session factory");
        }

        public NhRepository Open()
        {
            _session = SessionFactory.OpenSession();
            _transaction = _session.BeginTransaction();
            return this;
        }

        public T Save<T>(T entity) where T : Entity
        {
            _session.Save(entity);
            return entity;
        }

        public Domain.Utility.Queries.Query<T> Query<T>() where T : Entity
        {
            throw new NotImplementedException();
        }

        public IList<T> Satisfy<T>(Domain.Utility.Queries.Query<T> query)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            try
            {
                using (_transaction) { }
            }
            finally
            {
                try
                {
                    using (_session) { }
                }
                finally
                {
                    _transaction = null;
                    _session = null;
                }
            }
        }
    }
}
