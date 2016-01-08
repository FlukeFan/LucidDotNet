using System.Collections.Generic;

namespace Lucid.Domain.Execution
{
    public abstract class QueryList<TListItem> : IQueryList<TListItem>, IExecutable
    {
        public abstract IList<TListItem> List();

        object IExecutable.Execute()
        {
            return List();
        }
    }
}
