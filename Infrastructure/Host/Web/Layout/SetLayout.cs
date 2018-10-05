﻿using Lucid.Infrastructure.Lib.MvcApp.Pages;
using Lucid.Infrastructure.Lib.MvcApp.RazorFolders;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Lucid.Infrastructure.Host.Web.Layout
{
    public class SetLayout : ISetLayout
    {
        void ISetLayout.Set(IMvcAppPage page, PageInfo pageInfo, ViewContext viewContext)
        {
            viewContext.ViewBag.PageInfo = pageInfo;
            page.Layout = this.RelativeViewPath("Master.cshtml");
        }
    }
}
