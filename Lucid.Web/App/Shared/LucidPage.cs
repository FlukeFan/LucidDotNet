using System.Web.Mvc;

namespace Lucid.Web.App.Shared
{
    public abstract class LucidPage<T> : WebViewPage<T>
    {
        public void SetMenuPage(string title)
        {
            ViewBag.Title = title;
        }
    }
}