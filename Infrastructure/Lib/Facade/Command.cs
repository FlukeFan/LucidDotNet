using System.Threading.Tasks;

namespace Lucid.Infrastructure.Lib.Facade
{
    public abstract class Command<T> : ICommand<T>, IExecutable
    {
        public abstract Task<T> Execute();

        async Task<object> IExecutable.Execute()
        {
            return await Execute();
        }
    }
}
