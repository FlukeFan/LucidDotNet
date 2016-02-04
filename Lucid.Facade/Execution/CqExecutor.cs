using System.Collections.Generic;

namespace Lucid.Facade.Execution
{
    public class CqExecutor : ICqExecutor
    {
        private IExecutor _executor;

        public CqExecutor(IExecutor executor)
        {
            _executor = executor;
        }

        public void Execute(ICommand cmd)
        {
            _executor.Execute(cmd);
        }

        public TReturn Execute<TReturn>(ICommand<TReturn> cmd)
        {
            return (TReturn)_executor.Execute(cmd);
        }

        public IList<TListItem> Execute<TListItem>(IQueryList<TListItem> query)
        {
            return (IList<TListItem>)_executor.Execute(query);
        }

        public TReturn Execute<TReturn>(IQuerySingle<TReturn> query)
        {
            return (TReturn)_executor.Execute(query);
        }
    }
}
