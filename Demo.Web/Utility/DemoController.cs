using System;
using System.Web.Mvc;
using Lucid.Web;
using SimpleFacade;

namespace Demo.Web.Utility
{
    public abstract class DemoController : Controller
    {
        protected TReturn Exec<TReturn>(IQuery<TReturn> query)
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