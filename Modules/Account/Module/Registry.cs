using System.Threading.Tasks;
using Lucid.Infrastructure.Lib.Facade;
using Lucid.Infrastructure.Lib.Facade.Validation;
using Lucid.Infrastructure.Lib.MvcApp;
using Reposify;

namespace Lucid.Modules.Account
{
    public static class Registry
    {
        public static IExecutor Executor { get; set; }

        public static Task StartupAsync()
        {
            Executor =
                new ValidatingExecutor(
                    new Executor());

            return Task.CompletedTask;
        }

        public abstract class Entity : IEntity
        {
            object IEntity.Id => Id;

            public int Id { get; protected set; }
        }

        public abstract class Controller : MvcAppController
        {
            protected override IExecutor Executor() { return Registry.Executor; }
        }
    }
}
