using System;
using System.Web.Mvc;
using Lucid.Domain.Execution;

namespace Lucid.Web
{
    public static class MvcExecutor
    {
        public static ActionResult Execute<TReturn>(ICqExecutor executor, ICommand<TReturn> cmd, Func<TReturn, ActionResult> success, Func<ActionResult> failure)
        {
            var response = executor.Execute(cmd);
            return success(response);
        }
    }
}
