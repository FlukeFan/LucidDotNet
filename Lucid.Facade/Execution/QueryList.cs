using System.Collections.Generic;

namespace Lucid.Facade.Execution
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
