using System.Collections.Generic;

namespace Lucid.Domain.Execution
{
    public interface IQueryList<TListItem> { }

    public interface IHandleQueryList<TQuery, TListItem>
        where TQuery : IQueryList<TListItem>
    {
        IList<TListItem> List(TQuery query);
    }
}
