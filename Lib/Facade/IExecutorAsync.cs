using System.Threading.Tasks;

namespace Lucid.Lib.Facade
{
    public interface IExecutorAsync
    {
        Task<object> ExecuteAsync(IExecutionContext context);
    }
}
