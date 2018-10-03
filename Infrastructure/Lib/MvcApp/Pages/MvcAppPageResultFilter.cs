using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Lucid.Infrastructure.Lib.MvcApp.Pages
{
    public class MvcAppPageResultFilter : IResultFilter
    {
        private bool _launchDebugger;

        public MvcAppPageResultFilter(bool launchDebugger)
        {
            _launchDebugger = launchDebugger;
        }

        void IResultFilter.OnResultExecuting(ResultExecutingContext context)
        {
        }

        void IResultFilter.OnResultExecuted(ResultExecutedContext context)
        {
            var viewResult = context.Result as ViewResult;

            if (viewResult == null)
                return;

            var items = context.HttpContext.Items;

            if (items.ContainsKey(CustomRazorPageActivator.IsMvcAppPage) && !items.ContainsKey(MvcAppPage.SetupCalled))
            {
                if (_launchDebugger)
                {
                    // result is already sent, so the best we can do (in development) is force the debugger open
                    System.Diagnostics.Debugger.Launch();
                    System.Diagnostics.Debugger.Break();
                }

                // tests will catch this
                throw new Exception($"Setup was not called for MvcAppPage {viewResult.ViewName}");
            }
        }
    }
}
