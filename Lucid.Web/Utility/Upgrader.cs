using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
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

            var unzippedFolders = new List<string>();
            var unzippedFiles = new List<string>();

            using (var zipFile = new ZipFile(packagedZip))
                foreach (ZipEntry zipEntry in zipFile)
                {
                    var outFile = Path.Combine(webFolder, zipEntry.Name);
                    var folder = Path.GetDirectoryName(outFile);

                    if (!unzippedFolders.Contains(Normalise(folder)))
                        unzippedFolders.Add(Normalise(folder));

                    Directory.CreateDirectory(folder);

                    if (zipEntry.IsFile)
                    {
                        if (!unzippedFiles.Contains(Normalise(outFile)))
                            unzippedFiles.Add(Normalise(outFile));

                        using (var streamWriter = File.Create(outFile))
                            StreamUtils.Copy(zipFile.GetInputStream(zipEntry), streamWriter, buffer);
                    }
                }

            // delete any 'extra' files/folders if we're not in debug
            if (!HttpContext.Current.IsDebuggingEnabled)
            {
                // ignore extra files/folder in the root web folder (e.g., log files)
                if (unzippedFolders.Contains(Normalise(webFolder)))
                    unzippedFolders.Remove(Normalise(webFolder));

                var extraFolders = new List<string>();
                var extraFiles = new List<string>();

                foreach (var unzippedFolder in unzippedFolders.OrderByDescending(f => f.Length))
                {
                    foreach (var subFolder in Directory.GetDirectories(unzippedFolder))
                        if (!unzippedFolders.Contains(Normalise(subFolder)))
                            extraFolders.Add(subFolder);

                    foreach (var file in Directory.GetFiles(unzippedFolder))
                        if (!unzippedFiles.Contains(Normalise(file)))
                            extraFiles.Add(file);
                }

                foreach (var file in extraFiles)
                    Retry(() => File.Delete(file));

                foreach (var folder in extraFolders)
                    Retry(() => Directory.Delete(folder, true));
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

        private static string Normalise(string path)
        {
            path = path.Replace("\\", "/");
            path = path.TrimEnd('/');
            path = path.ToLower(); // should only do this on case-insensitive file systems
            return path;
        }

        private static void Retry(Action action)
        {
            for (var i = 0; i < 3; i++)
                try
                {
                    action();
                    return;
                }
                catch
                {
                    Thread.Sleep(10);
                }
        }
    }
}