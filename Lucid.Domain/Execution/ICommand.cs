namespace Lucid.Domain.Execution
{
    public interface ICommand {}

    public interface ICommand<TReturn> { }

    public interface IHandleVoidCommand<TCmd>
        where TCmd : ICommand
    {
        void Execute(TCmd cmd);
    }

    public interface IHandleCommand<TCmd, TReturn>
        where TCmd : ICommand<TReturn>
    {
        TReturn Execute(TCmd cmd);
    }
}
