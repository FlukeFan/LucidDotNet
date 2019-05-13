using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Lucid.Lib.MvcApp.Pages
{
    public interface ISetLayout
    {
        void Set(IRazorPage page, PageInfo pageInfo, ViewContext viewContext);
    }
}
