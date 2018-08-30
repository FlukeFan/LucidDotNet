using System.IO;

namespace Lucid.Infrastructure.Host.Mvc
{
    public class Logging
    {
        public static string PrepareConfigFile()
        {
            var sourceLogFile = Path.GetFullPath("nlog.config");

            if (!File.Exists(sourceLogFile))
                return null;

            var targetLogFile = Path.GetFullPath("../logs.config/nlog.mvc.config");

            var targetFolder = Path.GetDirectoryName(targetLogFile);

            if (!Directory.Exists(targetFolder))
                Directory.CreateDirectory(targetFolder);

            if (!File.Exists(targetLogFile))
                File.Copy(sourceLogFile, targetLogFile);

            var sourceModified = File.GetLastWriteTimeUtc(sourceLogFile);
            var targetModified = File.GetLastWriteTimeUtc(targetLogFile);

            if (targetModified > sourceModified)
                return targetLogFile;

            var previous = $"{targetLogFile}.previous";

            if (File.Exists(previous))
                File.Delete(previous);

            File.Move(targetLogFile, $"{targetLogFile}.previous");

            File.Copy(sourceLogFile, targetLogFile);

            return targetLogFile;
        }
    }
}
