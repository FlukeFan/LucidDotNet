using Demo.Domain.Utility;
using SimpleFacade.Execution;

namespace Demo.Infrastructure
{
    public class DemoExecutor : Executor
    {
        public DemoExecutor()
        {
            UsingHandlersFromAssemblyWithType<DemoEntity>();
        }
    }
}
