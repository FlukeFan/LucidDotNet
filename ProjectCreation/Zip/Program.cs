using System;
using System.IO;
using System.IO.Compression;

namespace Lucid.ProjectCreation.Zip
{
    class Program
    {
        static void Main(string[] args)
        {
            var rootFolder = Path.GetFullPath(args[0]);
            var zipFile = Path.GetFullPath(args[1]);
            var filesToZip = args[2].Split(';');

            using (var stream = File.Create(zipFile))
            using (var archive = new ZipArchive(stream, ZipArchiveMode.Create, false))
            {
                foreach (var fileToZip in filesToZip)
                {
                    var entryName = Path.GetFileName(fileToZip);
                    var entry = archive.CreateEntryFromFile(fileToZip, entryName);
                    Console.WriteLine($"Zipped {entryName}");
                }
            }
        }
    }
}
