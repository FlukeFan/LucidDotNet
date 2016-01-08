using System;
using System.Web.Mvc;
using Lucid.Domain.Execution;

namespace Demo.Web.Utility
{
    public abstract class DemoController : Controller
    {
        protected ActionResult Execute<T>(Command<T> cmd, Func<T, ActionResult> success, Func<ActionResult> failure)
        {
            return PresentationRegistry.Executor.Execute(cmd, success, failure);
        }
    }
}