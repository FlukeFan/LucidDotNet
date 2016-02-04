
namespace Lucid.Facade.Execution
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
