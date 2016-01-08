using System;
using System.Web.Mvc;
using Lucid.Domain.Execution;
using Lucid.Web;

namespace Demo.Web.Utility
{
    public abstract class DemoController : Controller
    {
        protected ActionResult Execute<TReturn>(ICommand<TReturn> cmd, Func<TReturn, ActionResult> success, Func<ActionResult> failure)
        {
            return MvcExecutor.Execute(PresentationRegistry.Executor, cmd, success, failure);
        }
    }
}