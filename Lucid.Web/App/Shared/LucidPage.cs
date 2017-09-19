using System.Web.Mvc;
using MvcForms;

namespace Lucid.Web.App.Shared
{
    public abstract class LucidPage<T> : WebViewPage<T>
    {
        public void SetLayout(string title)
        {
            ViewBag.Title = title;

            var user = Context.User;
            var isPjax = Request.IsPjax();

            var menuModel = new MenuModel(isPjax, user.Identity);
            ViewBag.MenuModel = menuModel;
            Layout = Views.Menu;
        }
    }
}