using System.IO;
using NLog.Web;

namespace Lucid.Infrastructure.Host.Mvc
{
    public class Logging
    {
        public static void Configure()
        {
            var sourceLogFile = Path.GetFullPath("nlog.config");

            if (!File.Exists(sourceLogFile))
                return;

            var targetLogFile = Path.GetFullPath("../nlog.mvc.config");

            if (!File.Exists(targetLogFile))
                File.Copy(sourceLogFile, targetLogFile);

            NLogBuilder.ConfigureNLog(targetLogFile);
        }
    }
}
