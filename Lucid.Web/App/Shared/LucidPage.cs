using System.Web.Mvc;
using MvcForms;

namespace Lucid.Web.App.Shared
{
    public abstract class LucidPage<T> : WebViewPage<T>
    {
        public void SetLayout(string title)
        {
            ViewBag.Title = title;

            Layout = Request.IsPjax()
                ? SharedViews.MasterPjax
                : SharedViews.Master;
        }
    }
}