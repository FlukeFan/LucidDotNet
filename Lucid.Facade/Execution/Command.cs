
namespace Lucid.Facade.Execution
{
    public abstract class Command : ICommand, IExecutable
    {
        public abstract void Execute();

        object IExecutable.Execute()
        {
            Execute();
            return null;
        }
    }

    public abstract class Command<TReturn> : ICommand<TReturn>, IExecutable
    {
        public abstract TReturn Execute();

        object IExecutable.Execute()
        {
            return Execute();
        }
    }
}
