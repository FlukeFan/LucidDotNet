using System.Collections.Generic;

namespace Lucid.Facade.Execution
{
    public interface IQueryList<TListItem> { }

    public interface IHandleQueryList<TQuery, TListItem>
        where TQuery : IQueryList<TListItem>
    {
        IList<TListItem> List(TQuery query);
    }
}
