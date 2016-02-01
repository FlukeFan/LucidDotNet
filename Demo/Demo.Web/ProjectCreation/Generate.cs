using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;

namespace Demo.Web.ProjectCreation
{
    public class Generate
    {
        public static readonly IList<string> GuidsToIgnore = new List<string>
        {
            "2150E333-8FDC-42A3-9474-1A3956D46DE8".ToUpper(), // project items
            "349c5851-65df-11da-9384-00065b846f21".ToUpper(), // web application
            "fae04ec0-301f-11d3-bf4b-00c04f79efbc".ToUpper(), // c#
        };

        public static readonly Regex GuidSearch = new Regex("(.{8}-.{4}-.{4}-.{4}-.{12})", RegexOptions.Compiled);

        private static readonly IList<string> _processedExtensions = new List<string>
        {
            ".sln",
            ".csproj",
        };

        private static readonly byte[] _buffer = new byte[4096];

        private static IDictionary<string, string> _guidMap;

        public static MemoryStream Project(Stream zipInputStream)
        {
            _guidMap = new Dictionary<string, string>();

            var memoryStream = new MemoryStream();

            using (var zipOutput = new ZipOutputStream(memoryStream))
            {
                zipOutput.IsStreamOwner = false;

                using (var zipInput = new ZipFile(zipInputStream))
                    foreach (ZipEntry zipEntry in zipInput)
                        CopyEntry(zipInput, zipEntry, zipOutput);
            }

            memoryStream.Position = 0;
            return memoryStream;
        }

        private static void CopyEntry(ZipFile zipInput, ZipEntry inputEntry, ZipOutputStream zipOutput)
        {
            if (!inputEntry.IsFile)
                return;

            var fileName = inputEntry.Name;
            using (var inputStream = zipInput.GetInputStream(inputEntry))
            {
                var extension = Path.GetExtension(fileName);

                var newEntry = new ZipEntry(fileName);
                newEntry.DateTime = inputEntry.DateTime;

                if (ShouldProcessFile(extension))
                    ProcessTextFile(inputStream, newEntry, zipOutput);
                else
                    ProcessBinaryFile(inputEntry, inputStream, newEntry, zipOutput);
            }
        }

        private static bool ShouldProcessFile(string extension)
        {
            return _processedExtensions.Contains(extension);
        }

        private static void ProcessBinaryFile(ZipEntry inputEntry, Stream inputFileStream, ZipEntry newEntry, ZipOutputStream zipOutput)
        {
            newEntry.Size = inputEntry.Size;

            zipOutput.PutNextEntry(newEntry);
            StreamUtils.Copy(inputFileStream, zipOutput, _buffer);
            zipOutput.CloseEntry();
        }

        private static void ProcessTextFile(Stream inputFileStream, ZipEntry newEntry, ZipOutputStream zipOutput)
        {
            var inputLines = new List<string>();
            using (var reader = new StreamReader(inputFileStream))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                    inputLines.Add(line);
            }

            var outputLines = new List<string>();

            foreach (var inputLine in inputLines)
            {
                var outputLine = inputLine;

                outputLine = ReplaceGuids(inputLine);

                outputLines.Add(outputLine);
            }

            var output = string.Join("\r\n", outputLines);
            var encoding = Encoding.UTF8;
            var bytes = encoding.GetBytes(output);
            newEntry.Size = bytes.Length;

            zipOutput.PutNextEntry(newEntry);
            zipOutput.Write(bytes, 0, bytes.Length);
            zipOutput.CloseEntry();
        }

        private static string ReplaceGuids(string inputLine)
        {
            var outputLine = inputLine;

            foreach (Match match in GuidSearch.Matches(outputLine))
            {
                var inputGuid = match.Value;

                if (GuidsToIgnore.Contains(inputGuid.ToUpper()))
                    continue;

                if (!_guidMap.ContainsKey(inputGuid))
                    _guidMap[inputGuid] = Guid.NewGuid().ToString().ToUpper();

                var replacement = _guidMap[inputGuid];
                var matchStartIndex = match.Index;
                var matchEndIndex = match.Index + match.Length;
                outputLine = outputLine.Substring(0, matchStartIndex) + replacement + outputLine.Substring(matchEndIndex, outputLine.Length - matchEndIndex);
            }

            return outputLine;
        }
    }
}