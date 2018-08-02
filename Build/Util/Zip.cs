using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace Lucid.Build.BuildUtil
{
    public class Zip : Command
    {
        public override void Execute(Stack<string> args)
        {
            if (args.Count != 3)
                throw new Exception($"usage: dotnet Build.dll Zip <root-folder> <zip-file> <list-of-files-separated-by-semicolon>");

            var rootFolder = Path.GetFullPath(args.Pop());
            var zipFile = Path.GetFullPath(args.Pop());
            var filesToZip = args.Pop().Split(';');

            using (var stream = File.Create(zipFile))
            using (var archive = new ZipArchive(stream, ZipArchiveMode.Create, false))
            {
                foreach (var fileToZip in filesToZip)
                {
                    var entryName = Path.GetRelativePath(rootFolder, fileToZip);
                    var entry = archive.CreateEntryFromFile(fileToZip, entryName);
                }
            }

            UsingConsoleColor(ConsoleColor.Green, () => Console.WriteLine($"Zipped {filesToZip.Length} files into {zipFile}"));
        }
    }
}
