using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Build.BuildUtil
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

            var packages = new List<Package>();
            CollectPackages(csproj, packages);

            var targetRoot = Path.Combine(Path.GetDirectoryName(csproj), "bin/content");

            if (Directory.Exists(targetRoot))
                Directory.Delete(targetRoot, true);

            Directory.CreateDirectory(targetRoot);

            CopyPackages(nugetRoot, packages, targetRoot);
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

        private void CollectPackages(string csproj, IList<Package> packages)
        {
            var projXml = new XmlDocument();
            projXml.Load(csproj);

            var packageReferences = projXml.SelectNodes("//*[local-name()='PackageReference']");

            foreach (XmlElement packageReference in packageReferences)
            {
                var packageName = packageReference.Attributes["Include"].Value;
                var version = packageReference.Attributes["Version"].Value;
                var package = new Package { Name = packageName, Version = version };
                packages.Add(package);
            }
        }

        private void CopyPackages(string nugetRoot, IList<Package> packages, string targetRoot)
        {
            foreach (var package in packages)
            {
                var nugetPackage = Path.Combine(nugetRoot, package.Name, package.Version);
                var targetFolder = Path.Combine(targetRoot, package.Name);
                CopyContent(nugetPackage, "content", targetFolder, package.Version);
                CopyContent(nugetPackage, "contentFiles", targetFolder, package.Version);
            }
        }

        private void CopyContent(string packageFolder, string contentName, string targetFolder, string version)
        {
            var source = Path.Combine(packageFolder, contentName);

            if (Directory.Exists(source))
            {
                CopyFolder(source, Path.Combine(targetFolder, contentName), version);
                UsingConsoleColor(ConsoleColor.Cyan, () => Console.WriteLine($"Copied NuGet package content {source}"));
            }
        }

        private void CopyFolder(string source, string target, string version)
        {
            // https://stackoverflow.com/a/8022011/357728
            // cos it's 2018, and we still have to write code to copy a directory *sigh*

            foreach (string dir in System.IO.Directory.GetDirectories(source, "*", SearchOption.AllDirectories))
                Directory.CreateDirectory(Path.Combine(target, dir.Substring(source.Length + 1)));

            foreach (string fileName in Directory.GetFiles(source, "*", SearchOption.AllDirectories))
                File.Copy(fileName, Path.Combine(target, fileName.Substring(source.Length + 1).Replace(version, "version")));
        }

        private class Package
        {
            public string Name;
            public string Version;
        }
    }
}
