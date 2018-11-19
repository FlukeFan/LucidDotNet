using System.Threading.Tasks;

namespace Lucid.Infrastructure.Lib.Facade
{
    public interface IExecutable
    {
        Task<object> Execute();
    }
}
