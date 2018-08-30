using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

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

            var existingVariables = ReadVariables(targetLogFile);
            var previous = $"{targetLogFile}.previous";

            if (File.Exists(previous))
                File.Delete(previous);

            File.Move(targetLogFile, $"{targetLogFile}.previous");

            var doc = XDocument.Load(sourceLogFile, LoadOptions.PreserveWhitespace);

            var variableElements = GetVariableElements(doc);

            foreach (var variableElement in variableElements)
            {
                var name = variableElement.Attribute("name").Value;

                if (!existingVariables.ContainsKey(name))
                    continue;

                variableElement.Attribute("value").Value = existingVariables[name];
            }

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
