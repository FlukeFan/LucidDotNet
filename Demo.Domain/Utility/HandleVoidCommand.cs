using System.Diagnostics;
using Lucid.Facade.Execution;

namespace Demo.Domain.Utility
{
    public abstract class HandleVoidCommand<TCmd> : IHandleVoidCommand<TCmd>
        where TCmd : ICommand
    {
        protected static IDemoRepository Repository {[DebuggerStepThrough] get { return Registry.Repository; } }

        public abstract void Execute(TCmd cmd);
    }
}
