using System;
using System.Web.Mvc;
using Lucid.Domain.Execution;

namespace Demo.Web.Utility
{
    public class MvcExecutor
    {
        private IExecutor _inner;

        public MvcExecutor(IExecutor inner)
        {
            _inner = inner;
        }

        public virtual ActionResult Execute<T>(Command<T> cmd, Func<T, ActionResult> success, Func<ActionResult> failure)
        {
            var response = (T)_inner.Execute(cmd);
            return success(response);
        }
    }
}