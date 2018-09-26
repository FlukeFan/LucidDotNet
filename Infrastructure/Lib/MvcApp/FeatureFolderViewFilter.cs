using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Lucid.Infrastructure.Lib.MvcApp
{
    public class FeatureFolderViewFilter : IActionFilter
    {
        private string _namespacePrefix;
        private string _modulePrefix;
        private string _viewPrefix;

        public FeatureFolderViewFilter(string namespacePrefix, string modulePrefix, string viewPrefix)
        {
            _namespacePrefix = namespacePrefix;
            _modulePrefix = modulePrefix;
            _viewPrefix = viewPrefix;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            var viewResult = context.Result as ViewResult;

            if (viewResult == null)
                return;

            var viewName = viewResult.ViewName;

            if (viewResult.ViewName == null)
            {
                var action = context.RouteData.Values["action"];
                viewName = $"{action}.cshtml";
            }

            var controllerPath = context.Controller.GetType().Namespace;

            if (controllerPath.StartsWith(_namespacePrefix))
                controllerPath = controllerPath.Substring(_namespacePrefix.Length);

            controllerPath = controllerPath.Replace('.', '/');

            viewResult.ViewName = $"{_viewPrefix}{controllerPath}/{_modulePrefix}{viewName}";
        }
    }
}
