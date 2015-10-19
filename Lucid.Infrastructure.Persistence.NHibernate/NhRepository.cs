using System;
using System.Collections.Generic;
using Lucid.Domain.Persistence;
using Lucid.Domain.Persistence.Queries;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Connection;
using NHibernate.Criterion;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Tool.hbm2ddl;

namespace Lucid.Infrastructure.Persistence.NHibernate
{
    public class NhRepository<TId> : IRepository<TId>, IDisposable
    {
        private static IDictionary<Type, Action<ICriteria, Where>> _restrictionProcessors = new Dictionary<Type, Action<ICriteria, Where>>();

        public static ISessionFactory SessionFactory { get; protected set; }

        static NhRepository()
        {
            _restrictionProcessors.Add(typeof(WhereBinaryComparison), (c, w) => AddBinaryComparison(c, (WhereBinaryComparison)w));
        }

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
            var criteria = _session.CreateCriteria(typeof(T));

            foreach (var restriction in query.Restrictions)
                AddRestriction(criteria, restriction);

            return criteria.List<T>();
        }

        private void AddRestriction(ICriteria criteria, Where where)
        {
            var whereType = where.GetType();

            if (!_restrictionProcessors.ContainsKey(whereType))
                throw new Exception("Unhandled Where clause: " + where);

            var processor = _restrictionProcessors[whereType];
            processor(criteria, where);
        }

        private static void AddBinaryComparison(ICriteria criteria, WhereBinaryComparison where)
        {
            criteria.Add(Restrictions.Eq(where.Operand1.Name, where.Operand2));
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
