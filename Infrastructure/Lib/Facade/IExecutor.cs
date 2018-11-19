using System.Threading.Tasks;

namespace Lucid.Infrastructure.Lib.Facade
{
    public interface IExecutor
    {
        Task<object> Execute(IExecutable executable);
    }
}
