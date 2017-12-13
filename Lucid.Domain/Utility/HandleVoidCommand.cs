using SimpleFacade;

namespace Lucid.Domain.Utility
{
    public abstract class HandleVoidCommand<TCmd> : Handler, IHandleVoidCommand<TCmd>
        where TCmd : ICommand
    {
        public abstract void Execute(TCmd cmd);
    }
}
