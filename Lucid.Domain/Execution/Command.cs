namespace Lucid.Domain.Execution
{
    public abstract class Command : IExecutable
    {
        public abstract void Execute();

        object IExecutable.Execute()
        {
            Execute();
            return null;
        }
    }

    public abstract class Command<T> : IExecutable
    {
        public abstract T Execute();

        object IExecutable.Execute()
        {
            return Execute();
        }
    }
}
