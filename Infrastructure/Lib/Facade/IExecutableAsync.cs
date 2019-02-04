using System.Threading.Tasks;

namespace Lucid.Infrastructure.Lib.Facade
{
    public interface IExecutableAsync
    {
        Task<object> ExecuteAsync();
    }
}
