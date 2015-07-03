using System;
using System.Collections.Generic;
using Lucid.Domain.Utility;
using Lucid.Domain.Utility.Queries;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Connection;
using NHibernate.Criterion;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Tool.hbm2ddl;

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
            });

            var mappings = Mapping.CreateMappings();
            config.AddDeserializedMapping(mappings, "DomainMapping");

            SchemaMetadataUpdater.QuoteTableAndColumns(config);
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

        public Query<T> Query<T>() where T : Entity
        {
            return new Query<T>(this);
        }

        public IList<T> Satisfy<T>(Query<T> query) where T : class
        {
            var criteria = _session.CreateCriteria<T>();

            foreach (var restriction in query.Restrictions)
                AddRestriction(criteria, restriction);

            return criteria.List<T>();
        }

        private void AddRestriction<T>(ICriteria criteria, Where<T> where)
        {
            var whereType = where.GetType().GetGenericTypeDefinition();

            if (whereType == typeof(WhereStringEqual<>))
                AddStringRestriction(criteria, (WhereStringEqual<T>)where);
            else
                throw new Exception("Unhandled restriction: " + where);
        }

        private void AddStringRestriction<T>(ICriteria criteria, WhereStringEqual<T> where)
        {
            criteria.Add(Restrictions.Eq(where.Property.Name, where.Value));
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
