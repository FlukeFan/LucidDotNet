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

            Layout = user.Identity.IsAuthenticated
                ? SharedViews.LoggedIn
                : SharedViews.LoggedOut;

            ViewBag.MasterLayout = Request.IsPjax()
                ? SharedViews.MasterPjax
                : SharedViews.Master;
        }
    }
}