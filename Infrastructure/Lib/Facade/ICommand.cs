using System.Threading.Tasks;

namespace Lucid.Infrastructure.Lib.Facade
{
    public interface ICommand<T>
    {
        Task<T> Execute();
    }
}
