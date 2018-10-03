using System;
using Lucid.Infrastructure.Lib.MvcApp.Pages;
using Microsoft.AspNetCore.Mvc.Razor;

namespace Lucid.Infrastructure.Lib.MvcApp
{
    public interface IMvcAppPage
    {
    }

    public static class MvcAppPage
    {
        public const string SetupCalled = "SetupCalled";
    }

    public abstract class MvcAppPage<TModel> : RazorPage<TModel>, IMvcAppPage
    {
        private PageInfo _pageInfo;

        public void Setup(Action<PageInfo> action)
        {
            _pageInfo = new PageInfo();
            action(_pageInfo);
            Context.Items.Add(MvcAppPage.SetupCalled, true);
        }
    }
}
