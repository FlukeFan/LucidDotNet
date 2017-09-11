using System.IO;
using System.Web;
using System.Web.Hosting;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;

namespace Lucid.Web.Utility
{
    public class Upgrader
    {
        private static bool _updating;

        public static bool InstalledNewVersion()
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

                    if (zipEntry.IsFile)
                        using (var streamWriter = File.Create(outFile))
                            StreamUtils.Copy(zipFile.GetInputStream(zipEntry), streamWriter, buffer);
                }

            var deployedZip = HostingEnvironment.MapPath("~/Deployed.zip");

            if (File.Exists(deployedZip))
                File.Delete(deployedZip);

            File.Move(packagedZip, deployedZip);

            // in case the app hasn't recycled
            HttpRuntime.UnloadAppDomain();

            return true;
        }

        public static bool BeginRequest(object source)
        {
            if (!_updating)
                return false;

            var app = (HttpApplication)source;
            var context = app.Context;

            // redirect back to the same URL until the update is complete
            context.Response.Redirect(context.Request.Url.ToString(), false);

            return true;
        }
    }
}