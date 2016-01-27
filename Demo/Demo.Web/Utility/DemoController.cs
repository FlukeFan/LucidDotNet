using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Lucid.Domain.Execution;
using Lucid.Web;

namespace Demo.Web.Utility
{
    public abstract class DemoController : Controller
    {
        protected IList<TListItem> Exec<TListItem>(IQueryList<TListItem> query)
        {
            return MvcExecutor.Exec(PresentationRegistry.Executor, query);
        }

        protected TReturn Exec<TReturn>(IQuerySingle<TReturn> query)
        {
            return MvcExecutor.Exec(PresentationRegistry.Executor, query);
        }

        protected ActionResult Exec(ICommand cmd, Func<ActionResult> success, Func<ActionResult> failure)
        {
            return MvcExecutor.Exec(ModelState, PresentationRegistry.Executor, cmd, success, failure);
        }

        protected ActionResult Exec<TReturn>(ICommand<TReturn> cmd, Func<TReturn, ActionResult> success, Func<ActionResult> failure)
        {
            return MvcExecutor.Exec(ModelState, PresentationRegistry.Executor, cmd, success, failure);
        }
    }
}