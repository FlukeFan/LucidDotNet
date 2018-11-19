namespace Lucid.Infrastructure.Lib.Facade
{
    public abstract class Command<T> : ICommand<T>, IExecutable
    {
        public abstract T Execute();

        object IExecutable.Execute()
        {
            return Execute();
        }
    }
}
