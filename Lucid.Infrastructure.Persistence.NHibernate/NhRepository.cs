using System;
using System.Collections.Generic;
using Lucid.Domain.Persistence;
using Lucid.Domain.Persistence.Queries;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Connection;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Tool.hbm2ddl;

namespace Lucid.Infrastructure.Persistence.NHibernate
{
    public class NhRepository<TId> : IRepository<TId>, IDisposable
    {
        public static ISessionFactory SessionFactory { get; protected set; }

        public static void Init(string connectionString, Type rootEntityType)
        {
            var config = new Configuration();

            config.DataBaseIntegration(db =>
            {
                db.ConnectionString = connectionString;
                db.ConnectionProvider<DriverConnectionProvider>();
                db.Driver<SqlClientDriver>();
                db.Dialect<MsSql2008Dialect>();
            });

            var mappings = Mapping.CreateMappings<TId>(rootEntityType);
            config.AddDeserializedMapping(mappings, "DomainMapping");

            SchemaMetadataUpdater.QuoteTableAndColumns(config);
            SessionFactory = config.BuildSessionFactory();
        }

        private ISession _session;
        private ITransaction _transaction;

        public NhRepository()
        {
            if (SessionFactory == null)
                throw new Exception("Call Init() once to setup the session factory");
        }

        public NhRepository<TId> Open()
        {
            _session = SessionFactory.OpenSession();
            _transaction = _session.BeginTransaction();
            return this;
        }

        public T Save<T>(T entity) where T : IEntity<TId>
        {
            _session.Save(entity);
            return entity;
        }

        public Query<T, TId> Query<T>() where T : IEntity<TId>
        {
            return new Query<T, TId>(this);
        }

        public IList<T> Satisfy<T>(Query<T, TId> query) where T : IEntity<TId>
        {
            var nhCriteria = NhCriteria.For(query);
            var criteria = nhCriteria.CreateCriteria(_session);
            return criteria.List<T>();
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
