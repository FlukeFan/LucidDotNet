using System.Threading;
using Lucid.Infrastructure.Lib.Domain;
using Lucid.Infrastructure.Lib.Domain.SqlServer;
using Lucid.Infrastructure.Lib.Facade;
using Lucid.Infrastructure.Lib.Facade.Validation;
using Lucid.Infrastructure.Lib.MvcApp;
using NHibernate;
using Reposify;

namespace Lucid.Modules.Account
{
    public class Registry : DomainRegistry
    {
        private static ISessionFactory _sessionFactory;

        public static AsyncLocal<INhSqlRepository>  Repository      = new AsyncLocal<INhSqlRepository>();
        public static IExecutorAsync                ExecutorAsync;

        public static void Startup(string connectionString)
        {
            _sessionFactory = BuildSessionFactory(connectionString);

            ExecutorAsync =
                new ValidatingExecutorAsync(
                    new RepositoryExecutorAsync(_sessionFactory, () => Repository.Value, r => Repository.Value = r,
                        new ExecutorAsync()));
        }

        public static ISessionFactory BuildSessionFactory(string connectionString)
        {
            return NhUtil.CreateNhSessionFactory<Registry.Entity>(connectionString);
        }

        public abstract class Entity : IEntity
        {
            object IEntity.Id => Id;
            public virtual int Id { get; protected set; }

            protected static INhSqlRepository Repository => Registry.Repository.Value;
        }

        public abstract class Controller : MvcAppController
        {
            protected override IExecutorAsync ExecutorAsync() { return Registry.ExecutorAsync; }
        }
    }
}
