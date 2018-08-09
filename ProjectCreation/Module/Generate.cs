using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Text.RegularExpressions;

namespace Lucid.ProjectCreation
{
    public class Generate
    {
        private static readonly IList<string> SkippedFiles = new List<string>
        {
            "LICENSE.txt",
        };

        private static readonly IList<string> ProcessedExtensions = new List<string>
        {
            ".gitignore",
            ".sln",
            ".csproj",
            ".proj",
            ".bat",
            ".nunit",
            ".txt",
            ".md",
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
            ".htm",
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

        public static readonly Regex GuidSearch     = new Regex("([^-]{8}-[^-]{4}-[^-]{4}-[^-]{4}-[^-]{12})", RegexOptions.Compiled);
        public static readonly Regex TempDirSearch  = new Regex("tempDirectory=\"[^\"]*\"", RegexOptions.Compiled);

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

            if (SkippedFiles.Contains(fileName))
                return;

            var bomEncoding = Encoding.ASCII;
            var shouldProcessFile = ShouldProcessFile(fileName);

            if (shouldProcessFile)
                using (var tmpStream = inputEntry.Open())
                    bomEncoding = ReadBomEncoding(tmpStream) ?? bomEncoding;

            using (var inputStream = inputEntry.Open())
            {
                var newEntry = zipOutput.CreateEntry(fileName);
                newEntry.LastWriteTime = inputEntry.LastWriteTime;

                if (shouldProcessFile)
                {
                    ProcessTextFile(inputStream, bomEncoding, newEntry, newName);
                }
                else
                {
                    ProcessBinaryFile(inputStream, newEntry);
                }
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

        private static void ProcessTextFile(Stream inputFileStream, Encoding bomEncoding, ZipArchiveEntry newEntry, string newName)
        {
            var inputLines = ReadLines(inputFileStream);
            var outputLines = new List<string>();

            foreach (var inputLine in inputLines)
            {
                var outputLine = ProcessLine(inputLine, newName);
                outputLines.Add(outputLine);
            }

            var output = string.Join("\n", outputLines);

            using (var outputStream = newEntry.Open())
            using (var streamWriter = new StreamWriter(outputStream, bomEncoding))
                streamWriter.Write(output);
        }

        public static string[] ReadLines(Stream inputFileStream)
        {
            using (var reader = new StreamReader(inputFileStream))
            {
                var content = reader.ReadToEnd();
                var inputLines = content.Split('\n');
                return inputLines;
            }
        }

        public static Encoding ReadBomEncoding(Stream stream)
        {
            var bom = new byte[4];
            stream.Read(bom, 0, 4);

            if (bom[0] == 0x2b && bom[1] == 0x2f && bom[2] == 0x76) return Encoding.UTF7;
            if (bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf) return new UTF8Encoding(true);
            if (bom[0] == 0xff && bom[1] == 0xfe) return Encoding.Unicode; //UTF-16LE
            if (bom[0] == 0xfe && bom[1] == 0xff) return Encoding.BigEndianUnicode; //UTF-16BE
            if (bom[0] == 0 && bom[1] == 0 && bom[2] == 0xfe && bom[3] == 0xff) return Encoding.UTF32;

            return null;
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

                if (Guids.ToIgnore.Contains(inputGuid))
                    continue;

                if (inputGuid.Contains("{8}-.{4}-.{4}-.{4}-.{12}"))
                    continue; // not a guid - it's finding the declaration of the Regex

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