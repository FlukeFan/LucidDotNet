using Lucid.Lib.MvcApp.Pages;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Lucid.Lib.Testing
{
    public class EmptySetLayout : ISetLayout
    {
        void ISetLayout.Set(IRazorPage page, PageInfo pageInfo, ViewContext viewContext)
        {
        }
    }
}
