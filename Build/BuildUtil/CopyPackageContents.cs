﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Lucid.Build.BuildUtil
{
    public class CopyPackageContents : Command
    {
        public override void Execute(Stack<string> args)
        {
            if (args.Count != 2)
                throw new Exception($"usage: dotnet Build.dll CopyPackageContents <csproj_file> <nuget_root>");

            var csproj = args.Pop();
            var nugetRoot = args.Pop();
            UsingConsoleColor(ConsoleColor.White, () => Console.WriteLine($"Unzipping package contents csproj='{csproj}' nuget-root='{nugetRoot}'"));

            var targetRoot = Path.Combine(Path.GetDirectoryName(csproj), "bin/content");
            var targetFlagFile = Path.Combine(targetRoot, "lastUpdated.flg");

            var sourceDate = ProjectLastUpdatedUtc(csproj);
            var targetDate = TargetLastUpdatedUtc(targetFlagFile);

            if (targetDate >= sourceDate)
            {
                UsingConsoleColor(ConsoleColor.Gray, () => Console.WriteLine($"Package contents in {targetRoot} are up to date."));
                return;
            }

            var packageFolders = new List<string>();
            CollectPackages(csproj, packageFolders);

            if (Directory.Exists(targetRoot))
                Directory.Delete(targetRoot, true);

            Directory.CreateDirectory(targetRoot);

            CopyPackages(nugetRoot, packageFolders, targetRoot);

            File.WriteAllText(targetFlagFile, "");
        }

        private DateTime ProjectLastUpdatedUtc(string csproj)
        {
            return File.GetLastWriteTimeUtc(csproj);
        }

        private DateTime TargetLastUpdatedUtc(string targetFlagFile)
        {
            return (File.Exists(targetFlagFile))
                ? File.GetLastWriteTimeUtc(targetFlagFile)
                : DateTime.MinValue;
        }

        private void CollectPackages(string csproj, IList<string> packageFolders)
        {
            var projXml = new XmlDocument();
            projXml.Load(csproj);

            var packageReferences = projXml.SelectNodes("//*[local-name()='PackageReference']");

            foreach (XmlElement packageReference in packageReferences)
            {
                var packageName = packageReference.Attributes["Include"].Value;
                var version = packageReference.Attributes["Version"].Value;
                var folder = Path.Combine(packageName, version);
                packageFolders.Add(folder);
            }
        }

        private void CopyPackages(string nugetRoot, IList<string> packageFolders, string targetRoot)
        {
            foreach (var packageFolder in packageFolders)
            {
                var nugetPackage = Path.Combine(nugetRoot, packageFolder);
                var targetFolder = Path.Combine(targetRoot, packageFolder);
                CopyContent(nugetPackage, "content", targetFolder);
                CopyContent(nugetPackage, "contentFiles", targetFolder);
            }
        }

        private void CopyContent(string packageFolder, string contentName, string targetFolder)
        {
            var source = Path.Combine(packageFolder, contentName);

            if (Directory.Exists(source))
            {
                CopyFolder(source, Path.Combine(targetFolder, contentName));
                UsingConsoleColor(ConsoleColor.Cyan, () => Console.WriteLine($"Copied NuGet package content {source}"));
            }
        }

        private void CopyFolder(string source, string target)
        {
            // https://stackoverflow.com/a/8022011/357728
            // cos it's 2018, and we still have to write code to copy a directory *sigh*

            foreach (string dir in System.IO.Directory.GetDirectories(source, "*", SearchOption.AllDirectories))
                Directory.CreateDirectory(Path.Combine(target, dir.Substring(source.Length + 1)));

            foreach (string fileName in Directory.GetFiles(source, "*", SearchOption.AllDirectories))
                File.Copy(fileName, Path.Combine(target, fileName.Substring(source.Length + 1)));
        }
    }
}