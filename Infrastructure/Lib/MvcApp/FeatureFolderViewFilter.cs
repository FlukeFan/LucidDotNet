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

            // need to decide if we're keeping this filter to handle
            // multiple (precompiled) views of the same name in different assemblies
        }
    }
}
