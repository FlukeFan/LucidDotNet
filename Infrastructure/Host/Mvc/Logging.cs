using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using NLog.Common;
using NLog.Targets;
using NLog.Targets.Wrappers;

namespace Lucid.Infrastructure.Host.Mvc
{
    public class Logging
    {
        /// <summary> Custom NLog buffer to batch emails in increasing batch sizes </summary>
        [Target("VariableBuffer", IsWrapper = true)]
        public class VariableBuffer : BufferingTargetWrapper
        {
            private DateTime    _lastWrite;
            private int         _waitFactor;

            public VariableBuffer()
            {
                _lastWrite = DateTime.MinValue;
                _waitFactor = 1;
                BufferSize = 100000;
                SlidingTimeout = false;
            }

            protected override void Write(AsyncLogEventInfo logEvent)
            {
                const int maxMins = 100 * 60 * 1000;
                const int reduceTimeoutMins = 30;

                var now = DateTime.UtcNow;

                if (FactorTimeout() < maxMins)
                    _waitFactor = _waitFactor * 2;

                var decreaseThreshold = _lastWrite + TimeSpan.FromMinutes(reduceTimeoutMins);

                while (_waitFactor > 1 && now > decreaseThreshold)
                {
                    _waitFactor = _waitFactor / 2;
                    decreaseThreshold += TimeSpan.FromMinutes(reduceTimeoutMins);
                }

                FlushTimeout = FactorTimeout();
                _lastWrite = now;

                InternalLogger.Trace($"VariableBuffer: FlushTimeout={FlushTimeout}");

                base.Write(logEvent);
            }

            private int FactorTimeout()
            {
                var timeout = _waitFactor * 1000;
                return timeout;
            }
        }

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
