
namespace Lucid.Facade.Execution
{
    public interface IQuery<in TReturn> { }

    public interface IHandleQuery<TQuery, TReturn>
        where TQuery : IQuery<TReturn>
    {
        TReturn Find(TQuery query);
    }
}
