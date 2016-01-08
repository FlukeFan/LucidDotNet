namespace Lucid.Domain.Execution
{
    public interface IQuerySingle<Return> { }

    public interface IHandleQuerySingle<Query, Return>
        where Query : IQuerySingle<Return>
    {
        Return Find(Query query);
    }
}
