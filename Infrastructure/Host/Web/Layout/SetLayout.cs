using Lucid.Infrastructure.Lib.MvcApp.Pages;
using Lucid.Infrastructure.Lib.MvcApp.RazorFolders;
using Microsoft.AspNetCore.Mvc.Rendering;
using MvcForms;

namespace Lucid.Infrastructure.Host.Web.Layout
{
    public class SetLayout : ISetLayout
    {
        void ISetLayout.Set(IMvcAppPage page, PageInfo pageInfo, ViewContext viewContext)
        {
            viewContext.ViewBag.PageInfo = pageInfo;

            var request = viewContext.HttpContext.Request;
            var isPjaxModal = request.IsPjaxModal();

            if (isPjaxModal)
            {
                page.Layout = this.RelativeViewPath("Modal.cshtml");
                return;
            }

            var isPjax = request.IsPjax();
            var menuModel = new MenuModel();

            menuModel.Layout = isPjax
                ? this.RelativeViewPath("MasterPjax.cshtml")
                : this.RelativeViewPath("Master.cshtml");

            viewContext.ViewBag.MenuModel = menuModel;
            page.Layout = this.RelativeViewPath("Menu.cshtml");
        }
    }
}
