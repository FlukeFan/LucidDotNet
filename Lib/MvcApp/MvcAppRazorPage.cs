using System;
using Lucid.Lib.MvcApp.Pages;
using Microsoft.AspNetCore.Mvc.Razor.Internal;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lucid.Lib.MvcApp
{
    public abstract class MvcAppRazorPage : PageBase
    {
        [RazorInject]
        protected ISetLayout SetLayout { get; set; }

        public void Setup(Action<PageInfo> action)
        {
            HttpContext.Items.Add(MvcAppPage.SetupCalled, true);
            var pageInfo = new PageInfo();
            action(pageInfo);
            SetLayout.Set(this, pageInfo, ViewContext);
        }
    }
}
