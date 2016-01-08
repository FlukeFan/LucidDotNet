namespace Lucid.Domain.Execution
{
    public abstract class QuerySingle<TReturn> : IQuerySingle<TReturn>, IExecutable
    {
        public abstract TReturn Find();

        object IExecutable.Execute()
        {
            return Find();
        }
    }
}
