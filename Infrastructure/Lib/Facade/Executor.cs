using System.Threading.Tasks;

namespace Lucid.Infrastructure.Lib.Facade
{
    public class Executor : IExecutor
    {
        Task<object> IExecutor.Execute(IExecutable executable)
        {
            return executable.Execute();
        }
    }
}
