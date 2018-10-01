using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace Lucid.Infrastructure.Host.Web.Logging
{
    public class LogConfig
    {
        public static string PrepareConfigFile(IList<string> logSetupMessages = null)
        {
            logSetupMessages = logSetupMessages ?? new List<string>();

            var sourceLogFile = Path.GetFullPath("nlog.config");

            if (!File.Exists(sourceLogFile))
            {
                logSetupMessages.Add($"No file found at {sourceLogFile}");
                return null;
            }

            var targetLogFile = Path.GetFullPath("../logs.config/nlog.web.config");

            var targetFolder = Path.GetDirectoryName(targetLogFile);

            if (!Directory.Exists(targetFolder))
                Directory.CreateDirectory(targetFolder);

            if (!File.Exists(targetLogFile))
                File.Copy(sourceLogFile, targetLogFile);

            var sourceModified = File.GetLastWriteTimeUtc(sourceLogFile);
            var targetModified = File.GetLastWriteTimeUtc(targetLogFile);

            logSetupMessages.Add($"sourceLogFile={sourceLogFile} sourceModified={sourceModified} targetLogFile={targetLogFile} targetModified={targetModified}");

            if (targetModified > sourceModified)
            {
                logSetupMessages.Add($"target log file is newer than source log file - not overwriting");
                return targetLogFile;
            }

            var existingVariables = ReadVariables(targetLogFile);
            var previous = $"{targetLogFile}.previous";

            if (File.Exists(previous))
            {
                logSetupMessages.Add($"deleting {previous}");
                File.Delete(previous);
            }

            logSetupMessages.Add($"moving {targetLogFile} to {previous}");
            File.Move(targetLogFile, $"{targetLogFile}.previous");

            var doc = XDocument.Load(sourceLogFile, LoadOptions.PreserveWhitespace);

            var variableElements = GetVariableElements(doc);

            foreach (var variableElement in variableElements)
            {
                var name = variableElement.Attribute("name").Value;

                if (!existingVariables.ContainsKey(name))
                    continue;

                logSetupMessages.Add($"retaining existing value for {name} in new log file");
                variableElement.Attribute("value").Value = existingVariables[name];
            }

            logSetupMessages.Add($"writing to {targetLogFile}");
            doc.Save(targetLogFile);

            return targetLogFile;
        }

        public static IDictionary<string, string> ReadVariables(string xmlFile)
        {
            var variables = new Dictionary<string, string>();

            var doc = XDocument.Load(xmlFile);
            var variableElements = GetVariableElements(doc);

            foreach (var variableElement in variableElements)
                variables.Add(variableElement.Attribute("name").Value, variableElement.Attribute("value").Value);

            return variables;
        }

        private static IEnumerable<XElement> GetVariableElements(XDocument doc)
        {
            var ns = doc.Root.GetDefaultNamespace();
            var variableElements = doc.Descendants(ns + "variable");
            return variableElements;
        }
    }
}
