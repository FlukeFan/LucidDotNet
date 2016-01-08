using System.Collections.Generic;

namespace Lucid.Domain.Execution
{
    public interface IQueryList<ListItem> { }

    public interface IHandleQueryList<Query, ListItem>
        where Query : IQueryList<ListItem>
    {
        IList<ListItem> List(Query query);
    }
}
