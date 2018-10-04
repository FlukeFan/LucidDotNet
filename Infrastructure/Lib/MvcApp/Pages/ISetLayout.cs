using Microsoft.AspNetCore.Mvc.Rendering;

namespace Lucid.Infrastructure.Lib.MvcApp.Pages
{
    public interface ISetLayout
    {
        void Set(IMvcAppPage page, PageInfo pageInfo, ViewContext viewContext);
    }
}
