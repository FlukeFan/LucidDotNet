using Lucid.Domain.Utility;
using SimpleFacade.Execution;

namespace Lucid.Infrastructure
{
    public class LucidExecutor : Executor
    {
        public LucidExecutor()
        {
            UsingHandlersFromAssemblyWithType<LucidEntity>();
        }
    }
}
