using System.Diagnostics;
using SimpleFacade;

namespace Lucid.Domain.Utility
{
    public abstract class HandleCommand<TCmd, TReturn> : IHandleCommand<TCmd, TReturn>
        where TCmd : ICommand<TReturn>
    {
        protected static ILucidRepository Repository {[DebuggerStepThrough] get { return Registry.Repository; } }

        public abstract TReturn Execute(TCmd cmd);
    }
}
