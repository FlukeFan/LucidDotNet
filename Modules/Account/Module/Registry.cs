using Lucid.Infrastructure.Lib.Domain.SqlServer;
using Lucid.Infrastructure.Lib.Facade;
using Lucid.Infrastructure.Lib.Facade.Validation;
using Lucid.Infrastructure.Lib.MvcApp;
using NHibernate;
using Reposify;

namespace Lucid.Modules.Account
{
    public static class Registry
    {
        private static ISessionFactory _sessionFactory;

        public static IExecutor Executor;

        public static void Startup(string connectionString)
        {
            _sessionFactory = NhUtil.CreateNhSessionFactory<Registry.Entity>(connectionString);

            Executor =
                new ValidatingExecutor(
                    new Executor());
        }

        public static ISessionFactory BuildSessionFactory(string connectionString)
        {
            return NhUtil.CreateNhSessionFactory<Registry.Entity>(connectionString);
        }

        public abstract class Entity : IEntity
        {
            object IEntity.Id => Id;
            public virtual int Id { get; protected set; }
        }

        public abstract class Controller : MvcAppController
        {
            protected override IExecutor Executor() { return Registry.Executor; }
        }
    }
}
