using System.Diagnostics;
using Lucid.Facade.Execution;

namespace Demo.Domain.Utility
{
    public abstract class HandleCommand<TCmd, TReturn> : IHandleCommand<TCmd, TReturn>
        where TCmd : ICommand<TReturn>
    {
        protected static IDemoRepository Repository {[DebuggerStepThrough] get { return Registry.Repository; } }

        public abstract TReturn Execute(TCmd cmd);
    }
}
