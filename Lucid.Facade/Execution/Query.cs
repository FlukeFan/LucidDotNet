
namespace Lucid.Facade.Execution
{
    public abstract class Query<TReturn> : IQuery<TReturn>, IExecutable
    {
        public abstract TReturn Find();

        object IExecutable.Execute()
        {
            return Find();
        }
    }
}
