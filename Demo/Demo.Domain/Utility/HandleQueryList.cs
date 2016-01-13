using System.Collections.Generic;
using System.Diagnostics;
using Lucid.Domain.Execution;

namespace Demo.Domain.Utility
{
    public abstract class HandleQueryList<TQuery, TListItem> : IHandleQueryList<TQuery, TListItem>
        where TQuery : IQueryList<TListItem>
    {
        protected static IDemoRepository Repository {[DebuggerStepThrough] get { return Registry.Repository; } }

        public abstract IList<TListItem> List(TQuery query);
    }
}
