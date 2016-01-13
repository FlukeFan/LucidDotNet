using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Lucid.Domain.Execution;

namespace Lucid.Web
{
    public static class MvcExecutor
    {
        public static IList<TListItem> Exec<TListItem>(ICqExecutor executor, IQueryList<TListItem> query)
        {
            return executor.Execute(query);
        }

        public static TReturn Exec<TReturn>(ICqExecutor executor, IQuerySingle<TReturn> query)
        {
            return executor.Execute(query);
        }

        public static ActionResult Exec(ICqExecutor executor, ICommand cmd, Func<ActionResult> success, Func<ActionResult> failure)
        {
            executor.Execute(cmd);
            return success();
        }

        public static ActionResult Exec<TReturn>(ICqExecutor executor, ICommand<TReturn> cmd, Func<TReturn, ActionResult> success, Func<ActionResult> failure)
        {
            var response = executor.Execute(cmd);
            return success(response);
        }
    }
}
