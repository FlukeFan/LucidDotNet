using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Lucid.Infrastructure.Lib.MvcApp.RazorFolders
{
    public class FeatureFolderViewFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            var viewResult = context.Result as ViewResult;

            if (viewResult == null)
                return;

            var viewName = viewResult.ViewName;

            if (viewName != null && viewName.StartsWith("~/"))
                return; // allow controllers to explicitly choose the view using "~/..."

            if (viewName == null)
            {
                var action = context.RouteData.Values["action"];
                viewName = $"/{action}.cshtml";
            }

            var controllerType = context.Controller.GetType();
            viewName = controllerType.RelativeViewPath(viewName);

            viewResult.ViewName = viewName;
        }
    }
}
