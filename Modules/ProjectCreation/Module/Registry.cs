using Lucid.Infrastructure.Lib.Facade;
using Lucid.Infrastructure.Lib.Facade.Validation;
using Lucid.Infrastructure.Lib.MvcApp;

namespace Lucid.Modules.ProjectCreation
{
    public static class Registry
    {
        public static IExecutor Executor;

        public static void Startup()
        {
            Executor =
                new ValidatingExecutor(
                    new Executor());
        }

        public class ProjectCreationController : MvcAppController
        {
            protected override IExecutor Executor() { return Registry.Executor; }
        }
    }
}
