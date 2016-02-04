using Demo.Domain.Utility;
using Lucid.Facade.Execution;

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
