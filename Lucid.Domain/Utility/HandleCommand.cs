using SimpleFacade;

namespace Lucid.Domain.Utility
{
    public abstract class HandleCommand<TCmd, TReturn> : Handler, IHandleCommand<TCmd, TReturn>
        where TCmd : ICommand<TReturn>
    {
        public abstract TReturn Execute(TCmd cmd);
    }
}
