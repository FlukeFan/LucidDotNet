namespace Lucid.Domain.Execution
{
    public abstract class QuerySingle<T> : IExecutable
    {
        public abstract T Find();

        object IExecutable.Execute()
        {
            return Find();
        }
    }
}
