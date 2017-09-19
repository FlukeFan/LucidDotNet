using System.Web.Mvc;
using MvcForms;

namespace Lucid.Web.App.Shared
{
    public class LayoutFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var viewResult = filterContext.Result as ViewResult;

            if (viewResult == null)
                return;

            var context = filterContext.HttpContext;
            var user = context.User;
            var isPjax = context.Request.IsPjax();

            var menuModel = new MenuModel(isPjax, user.Identity);
            viewResult.ViewBag.MenuModel = menuModel;
            viewResult.MasterName = Views.Menu;
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
        }
    }
}