using Lucid.Domain.Utility;
using SimpleFacade.Execution;

namespace Lucid.Infrastructure
{
    public class DemoExecutor : Executor
    {
        public DemoExecutor()
        {
            UsingHandlersFromAssemblyWithType<LucidEntity>();
        }
    }
}
