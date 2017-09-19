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

            if (user.Identity.IsAuthenticated)
            {
                // logged in
                ViewBag.LoggedInBackground = "#cfc";
                ViewBag.IdentityText = user.Identity.Name;
            }
            else
            {
                // logged out
                ViewBag.LoggedInBackground = "#ccc";
                ViewBag.IdentityText = "Logged Out";
            }

            Layout = Views.Menu;

            ViewBag.MasterLayout = Request.IsPjax()
                ? Views.MasterPjax
                : Views.Master;
        }
    }
}