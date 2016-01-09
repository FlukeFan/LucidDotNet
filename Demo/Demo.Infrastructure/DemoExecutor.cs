using Demo.Domain.Utility;
using Lucid.Domain.Execution;

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
