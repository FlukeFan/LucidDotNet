using Microsoft.AspNetCore.Mvc.Razor;

namespace Lucid.Infrastructure.Lib.MvcApp
{
    public abstract class MvcAppPage<TModel> : RazorPage<TModel>
    {
        public void SetTitle(string title)
        {
        }
    }
}
