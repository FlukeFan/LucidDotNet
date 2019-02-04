using System.Threading.Tasks;

namespace Lucid.Infrastructure.Lib.Facade
{
    public abstract class CommandAsync<TReturn> : ICommand<TReturn>, IExecutableAsync
    {
        public abstract Task<TReturn> ExecuteAsync();

        async Task<object> IExecutableAsync.ExecuteAsync()
        {
            return await ExecuteAsync();
        }
    }
}
