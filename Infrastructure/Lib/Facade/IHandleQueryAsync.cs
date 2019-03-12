using System.Threading.Tasks;

namespace Lucid.Infrastructure.Lib.Facade
{
    public interface IHandleQueryAsync<TQuery, TReturn>
        where TQuery : IQuery<TReturn>
    {
        Task<TReturn> Find(TQuery query);
    }
}
