namespace Lucid.Domain.Remote
{
    public abstract class Command : IRemoteable
    {
        public abstract void Execute();

        object IRemoteable.Execute()
        {
            Execute();
            return null;
        }
    }

    public abstract class Command<T> : IRemoteable
    {
        public abstract T Execute();

        object IRemoteable.Execute()
        {
            return Execute();
        }
    }
}
