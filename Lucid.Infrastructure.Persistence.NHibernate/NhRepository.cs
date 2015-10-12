using System;
using System.Collections.Generic;
using System.Linq;
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
        private static IDictionary<Type, Type> _whereInterfaces = new Dictionary<Type, Type>();

        private static IDictionary<Type, Action<ICriteria, IWhere>> _restrictionProcessors = new Dictionary<Type, Action<ICriteria, IWhere>>();

        public static ISessionFactory SessionFactory { get; protected set; }

        static NhRepository()
        {
            _restrictionProcessors.Add(typeof(IWhereStringEqual), (c, w) => AddStringRestriction(c, (IWhereStringEqual)w));
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

        public Query<T, TId> Query<T>() where T : class, IEntity<TId>
        {
            return new Query<T, TId>(this);
        }

        public IList<T> Satisfy<T>(Query<T, TId> query) where T : class, IEntity<TId>
        {
            var criteria = _session.CreateCriteria<T>();

            foreach (var restriction in query.Restrictions)
                AddRestriction(criteria, restriction);

            return criteria.List<T>();
        }

        private void AddRestriction<T>(ICriteria criteria, Where<T> where)
        {
            var type = where.GetType();
            var genericType = type.IsGenericType ? type.GetGenericTypeDefinition() : type;

            if (!_whereInterfaces.ContainsKey(type))
            {
                var interfaces = genericType.GetInterfaces().Where(t => t != typeof(IWhere)).ToList();

                if (interfaces.Count != 1)
                    throw new Exception("Could not determine single interface for where restriction: " + type);

                _whereInterfaces.Add(type, interfaces[0]);
            }

            var whereInterface = _whereInterfaces[type];

            if (!_restrictionProcessors.ContainsKey(whereInterface))
                throw new Exception("Not handled: " + whereInterface);

            _restrictionProcessors[whereInterface](criteria, where);
        }

        private static void AddStringRestriction(ICriteria criteria, IWhereStringEqual where)
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
