using System.Threading.Tasks;

namespace Lucid.Infrastructure.Lib.Facade
{
    public abstract class Command<TReturn> : ICommand<TReturn>, IExecutable
    {
        public abstract Task<TReturn> Execute();

        async Task<object> IExecutable.Execute()
        {
            return await Execute();
        }
    }
}
