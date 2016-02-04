using System.Collections.Generic;

namespace Lucid.Facade.Execution
{
    public interface ICqExecutor
    {
        void                Execute(ICommand cmd);
        TReturn             Execute<TReturn>(ICommand<TReturn> cmd);
        IList<TListItem>    Execute<TListItem>(IQueryList<TListItem> query);
        TReturn             Execute<TReturn>(IQuerySingle<TReturn> query);
    }
}
