using Lucid.Lib.Facade;
using Lucid.Lib.Facade.Validation;
using Lucid.Lib.MvcApp;

namespace Lucid.Modules.ProjectCreation
{
    public static class Registry
    {
        public static IExecutorAsync ExecutorAsync;

        public static void Startup()
        {
            ExecutorAsync =
                new ValidatingExecutorAsync(
                    new ExecutorAsync());
        }

        public class ProjectCreationController : MvcAppController
        {
            protected override IExecutorAsync ExecutorAsync() { return Registry.ExecutorAsync; }
        }
    }
}
