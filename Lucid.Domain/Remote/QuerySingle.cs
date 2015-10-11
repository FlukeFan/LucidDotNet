namespace Lucid.Domain.Remote
{
    public abstract class QuerySingle<T> : IRemoteable
    {
        public abstract T Execute();

        object IRemoteable.Execute()
        {
            return Execute();
        }
    }
}
