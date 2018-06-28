using System;
using Lucid.Web.Utility;
using MvcFolders;

namespace Lucid.Web
{
    public class LucidApplication : System.Web.HttpApplication
    {
        public static Startup Startup = new Startup();

        protected void Application_Start()
        {
            if (Upgrader.InstalledNewVersion())
                return;

            Startup.Init();

            FeatureFolders.Register(typeof(LucidController).Assembly, "Lucid.Web.App");
        }

        protected void Application_BeginRequest(Object source, EventArgs e)
        {
            if (Upgrader.BeginRequest(source))
                return;
        }
    }
}