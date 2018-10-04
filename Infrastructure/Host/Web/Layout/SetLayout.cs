using Lucid.Infrastructure.Lib.MvcApp.Pages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Lucid.Infrastructure.Host.Web.Layout
{
    public class SetLayout : ISetLayout
    {
        void ISetLayout.Set(IMvcAppPage page, PageInfo pageInfo, ViewContext viewContext)
        {
            page.Layout = "/Lucid.Infrastructure.Host.Web/Layout/Master.cshtml";
        }
    }
}
