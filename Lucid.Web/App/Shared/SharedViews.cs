﻿using System.Web.Mvc;
using MvcForms;

namespace Lucid.Web.App.Shared
{
    public static class SharedViews
    {
        public const string Master      = "~/App/Shared/_master.cshtml";
        public const string MasterPjax  = "~/App/Shared/_masterPjax.cshtml";
        public const string LoggedOut   = "~/App/Shared/_loggedOut.cshtml";
        public const string LoggedIn    = "~/App/Shared/_loggedIn.cshtml";

        public static void SetMasterLayout(WebViewPage page)
        {
            page.Layout = page.Request.IsPjax()
                ? MasterPjax
                : Master;
        }
    }
}