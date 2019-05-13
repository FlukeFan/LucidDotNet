using System.Threading.Tasks;

namespace Lucid.Lib.Facade
{
    public interface IExecutableAsync
    {
        Task<object> ExecuteAsync();
    }
}
