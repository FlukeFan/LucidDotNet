using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Text.RegularExpressions;

namespace Lucid.Web.ProjectCreation
{
    public class Generate
    {
        private static readonly IList<string> ProcessedExtensions = new List<string>
        {
            ".gitignore",
            ".sln",
            ".csproj",
            ".proj",
            ".bat",
            ".nunit",
            ".txt",
            ".targets",
            ".json",
            ".config",
            ".js",
            ".cs",
            ".cshtml",
            ".asax",
            ".css",
            ".scss",
            ".defaults",
            ".html",
        };

        private static readonly IList<String> UnprocessedExtensions = new List<string>
        {
            ".xsd",
            ".exe",
            ".eot",
            ".svg",
            ".ttf",
            ".woff",
            ".woff2",
            ".map",
            ".rtf",
            ".ico",
        };

        public static readonly IList<string> GuidsToIgnore = new List<string>
        {
            "2150E333-8FDC-42A3-9474-1A3956D46DE8".ToUpper(), // solution folder
            "EE62401E-B98D-47D7-A63E-B70A60117F30".ToUpper(), // solution folder
            "349c5851-65df-11da-9384-00065b846f21".ToUpper(), // web application
            "fae04ec0-301f-11d3-bf4b-00c04f79efbc".ToUpper(), // c# project
            "D27CDB6E-AE6D-11cf-96B8-444553540000".ToUpper(), // jQuery reference to legacy flash object
        };

        public static readonly IList<string> GuidsToReplace = new List<string>
        {
            "E47CBDCF-B71C-4A7B-BC83-B4649FC99361".ToUpper(), // Items folder
            "2C74567A-5CF2-4006-B6C1-FDA122578AD1".ToUpper(), // Lucid.Database
            "C4352D6D-C6DF-4A1A-A613-800B3573F8A3".ToUpper(), // Lucid.Database.Tests
            "2D85D27E-BDFF-4545-A48C-49D193143232".ToUpper(), // Lucid.Domain.Contract
            "74BDA17E-FC05-42F2-810C-D7B677C0D29E".ToUpper(), // Lucid.Domain
            "CC13E71E-1978-4232-93F3-C7380D498C9C".ToUpper(), // Lucid.Domain.Tests
            "C18D5B86-C73A-483B-9405-39C8D85A1823".ToUpper(), // Lucid.Infrastructure
            "3877B1C6-15A4-43D3-899E-74168842C92C".ToUpper(), // Lucid.Infrastructure.Tests
            "9EA489D1-14A1-4482-872B-3A1FD7F93646".ToUpper(), // Lucid.Web
            "098ADBA2-8686-4E64-9937-EC63DFA11432".ToUpper(), // Lucid.Web.Tests
            "B21A7182-EF27-48FA-93D8-C8E24E44FDB6".ToUpper(), // Lucid.System.Tests
        };

        public static readonly Regex GuidSearch     = new Regex("([^-]{8}-[^-]{4}-[^-]{4}-[^-]{4}-[^-]{12})", RegexOptions.Compiled);
        public static readonly Regex TempDirSearch  = new Regex("tempDirectory=\"[^\"]*\"", RegexOptions.Compiled);

        private static readonly byte[] _buffer = new byte[4096];

        private static IDictionary<string, string> _guidMap;

        public static MemoryStream Project(Stream zipInputStream, string newName)
        {
            _guidMap = new Dictionary<string, string>();

            var memoryStream = new MemoryStream();

            using (var zipOutput = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
            {
                using (var zipInput = new ZipArchive(zipInputStream, ZipArchiveMode.Read))
                    foreach (var zipEntry in zipInput.Entries)
                        CopyEntry(zipInput, zipEntry, zipOutput, newName);
            }

            memoryStream.Position = 0;
            return memoryStream;
        }

        public static bool ShouldProcessFile(string fileName)
        {
            if (fileName.ToLower().Replace('\\', '/').Contains("packages/"))
                return false;

            var extension = Path.GetExtension(fileName).ToLower();

            if (ProcessedExtensions.Contains(extension))
                return true;

            if (!UnprocessedExtensions.Contains(extension))
                throw new Exception(string.Format("Extension {0} should be added to Generate.ProcessedExtensions or Generate.UnprocessedExtensions.", extension));

            return false;
        }

        private static void CopyEntry(ZipArchive zipInput, ZipArchiveEntry inputEntry, ZipArchive zipOutput, string newName)
        {
            var fileName = inputEntry.FullName.Replace("Lucid", newName).Replace("lucid", ToCamelCase(newName));
            using (var inputStream = inputEntry.Open())
            {
                var newEntry = zipOutput.CreateEntry(fileName);
                newEntry.LastWriteTime = inputEntry.LastWriteTime;

                if (ShouldProcessFile(fileName))
                    ProcessTextFile(inputStream, newEntry, newName);
                else
                    ProcessBinaryFile(inputStream, newEntry);
            }
        }

        private static string ToCamelCase(string pascalCase)
        {
            return char.ToLower(pascalCase[0]) + pascalCase.Substring(1);
        }

        private static void ProcessBinaryFile(Stream inputFileStream, ZipArchiveEntry newEntry)
        {
            using (var newStream = newEntry.Open())
                inputFileStream.CopyTo(newStream);
        }

        private static void ProcessTextFile(Stream inputFileStream, ZipArchiveEntry newEntry, string newName)
        {
            var inputLines = ReadLines(inputFileStream);
            var outputLines = new List<string>();

            foreach (var inputLine in inputLines)
            {
                var outputLine = ProcessLine(inputLine, newName);
                outputLines.Add(outputLine);
            }

            var output = string.Join("\r\n", outputLines);
            var encoding = Encoding.UTF8;
            var bytes = encoding.GetBytes(output);

            using (var outputStream = newEntry.Open())
                outputStream.Write(bytes, 0, bytes.Length);
        }

        public static string[] ReadLines(Stream inputFileStream)
        {
            using (var reader = new StreamReader(inputFileStream))
            {
                var content = reader.ReadToEnd();
                var inputLines = content.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.None);
                return inputLines;
            }
        }

        public static string ProcessLine(string inputLine, string newName)
        {
            var outputLine = inputLine;

            outputLine = ReplaceGuids(outputLine);
            outputLine = ReplaceLucid(outputLine, newName);
            outputLine = ReplaceHashes(outputLine);
            outputLine = ReplaceAppHarborTmpDir(outputLine);

            return outputLine;
        }

        private static string ReplaceGuids(string inputLine)
        {
            var outputLine = inputLine;

            foreach (Match match in GuidSearch.Matches(outputLine))
            {
                var inputGuid = match.Value.ToUpper();

                if (GuidsToIgnore.Contains(inputGuid))
                    continue;

                if (inputGuid.Contains("{8}-.{4}-.{4}-.{4}-.{12}"))
                    continue; // not a guid - it's finding the declaration of the Regex

                if (!GuidsToReplace.Contains(inputGuid))
                {
                    var errorMessage = string.Format("Could not find guid {0}.  All project GUIDS must be defined in Generate.GuidsToIngore or GuidsToReplace.  Line: {1}", inputGuid, inputLine);
                    Console.WriteLine(errorMessage);
                    throw new Exception(errorMessage);
                }

                if (!_guidMap.ContainsKey(inputGuid))
                    _guidMap[inputGuid] = Guid.NewGuid().ToString().ToUpper();

                var replacement = _guidMap[inputGuid];
                var matchStartIndex = match.Index;
                var matchEndIndex = match.Index + match.Length;
                outputLine = outputLine.Substring(0, matchStartIndex) + replacement + outputLine.Substring(matchEndIndex, outputLine.Length - matchEndIndex);
            }

            return outputLine;
        }

        private static string ReplaceLucid(string inputLine, string newName)
        {
            var outputLine = inputLine.Replace("Lucid", newName);
            outputLine = outputLine.Replace("lucid", ToCamelCase(newName));
            return outputLine;
        }

        private static string ReplaceHashes(string inputLine)
        {
            var hashCode = "Hashes.Add(typeof(";

            if (!inputLine.Contains(hashCode))
                return inputLine;

            var outputLine = inputLine.Replace(hashCode, "// " + hashCode);
            outputLine += "// Replace with 'real' hashes once reployed.";
            return outputLine;
        }

        private static string ReplaceAppHarborTmpDir(string inputLine)
        {
            var match = TempDirSearch.Match(inputLine);

            if (!match.Success)
                return inputLine;

            var outputLine = inputLine.Substring(0, match.Index) + inputLine.Substring(match.Index + match.Length);
            return outputLine;
        }
    }
}