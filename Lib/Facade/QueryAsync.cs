using System.Threading.Tasks;

namespace Lucid.Lib.Facade
{
    public abstract class QueryAsync<TReturn> : IQuery<TReturn>, IExecutableAsync
    {
        public abstract Task<TReturn> ExecuteAsync();

        async Task<object> IExecutableAsync.ExecuteAsync()
        {
            return await ExecuteAsync();
        }
    }
}
