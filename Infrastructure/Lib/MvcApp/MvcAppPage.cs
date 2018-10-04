using System;
using Lucid.Infrastructure.Lib.MvcApp.Pages;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Razor.Internal;

namespace Lucid.Infrastructure.Lib.MvcApp
{
    public static class MvcAppPage
    {
        public const string SetupCalled = "SetupCalled";
    }

    public abstract class MvcAppPage<TModel> : RazorPage<TModel>, IMvcAppPage
    {
        [RazorInject]
        protected ISetLayout SetLayout { get; set; }

        private PageInfo _pageInfo;

        public void Setup(Action<PageInfo> action)
        {
            Context.Items.Add(MvcAppPage.SetupCalled, true);
            _pageInfo = new PageInfo();
            action(_pageInfo);
            SetLayout.Set(this, _pageInfo, ViewContext);
        }
    }
}
