using System;
using System.IO;
using System.Web;
using System.Web.Hosting;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using Lucid.Web.Utility;
using MvcFolders;

namespace Lucid.Web
{
    public class LucidApplication : System.Web.HttpApplication
    {
        private static bool _updating;

        public static Startup Startup = new Startup();

        protected void Application_Start()
        {
            if (NewVersion())
                return;

            Startup.Init();

            FeatureFolders.Register(typeof(LucidController).Assembly, "Lucid.Web.App");
        }

        private bool NewVersion()
        {
            var packagedZip = HostingEnvironment.MapPath("~/Packaged.zip");

            if (!File.Exists(packagedZip))
                return false;

            _updating = true;

            var webFolder = HostingEnvironment.MapPath("~");
            var buffer = new byte[4096];

            using (var zipFile = new ZipFile(packagedZip))
                foreach (ZipEntry zipEntry in zipFile)
                {
                    var outFile = Path.Combine(webFolder, zipEntry.Name);
                    Directory.CreateDirectory(Path.GetDirectoryName(outFile));

                    using (var streamWriter = File.Create(outFile))
                        StreamUtils.Copy(zipFile.GetInputStream(zipEntry), streamWriter, buffer);
                }

            var deployedZip = HostingEnvironment.MapPath("~/Deployed.zip");

            if (File.Exists(deployedZip))
                File.Delete(deployedZip);

            File.Move(packagedZip, deployedZip);

            return true;
        }

        protected void Application_BeginRequest(Object source, EventArgs e)
        {
            if (_updating)
            {
                var app = (HttpApplication)source;
                var context = app.Context;

                context.Response.Redirect(context.Request.Url.ToString(), false);
            }
        }
    }
}