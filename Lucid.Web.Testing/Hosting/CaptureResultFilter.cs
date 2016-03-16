using System.Web.Mvc;

namespace Lucid.Web.Testing.Hosting
{
    public class CaptureResultFilter : ActionFilterAttribute
    {
        public static ResultExecutedContext LastResult;

        public static void Register()
        {
            GlobalFilters.Filters.Add(new CaptureResultFilter());
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            base.OnResultExecuted(filterContext);
            LastResult = filterContext;
        }
    }
}