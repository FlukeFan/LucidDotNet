namespace Lucid.Domain.Execution
{
    public interface IQuerySingle<TReturn> { }

    public interface IHandleQuerySingle<TQuery, TReturn>
        where TQuery : IQuerySingle<TReturn>
    {
        TReturn Find(TQuery query);
    }
}
