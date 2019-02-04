using System.Threading.Tasks;

namespace Lucid.Infrastructure.Lib.Facade
{
    public interface IExecutorAsync
    {
        Task<object> ExecuteAsync(IExecutionContext context);
    }
}
