namespace Lucid.Domain.Execution
{
    public interface ICommand {}

    public interface ICommand<Returns> { }

    public interface IHandleVoidCommand<Cmd>
        where Cmd : ICommand
    {
        void Execute(Cmd cmd);
    }

    public interface IHandleCommand<Cmd, Return>
        where Cmd : ICommand<Return>
    {
        Return Execute(Cmd cmd);
    }
}
