﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using Demo.Web.ProjectCreation;
using FluentAssertions;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using NUnit.Framework;

namespace Demo.System.Tests.ProjectCreation
{
    [TestFixture]
    [Explicit("Until this is working")]
    public class GenerateProjectTests
    {
        [Test]
        public void Execute()
        {
            var cmd = new GenerateProject { Name = "ShinyNewProject" };

            var zipBytes = cmd.Execute();

            var originalFolder = @"..\..\..";
            var buildFolder = Path.Combine(Path.GetTempPath(), "GpTmp");
            Console.WriteLine("Building project {0} in {1}", cmd.Name, buildFolder);
            Unzip(zipBytes, buildFolder);
            CopyFolder(@"..\..\..\packages", Path.Combine(buildFolder, "packages"));
            File.WriteAllText(Path.Combine(buildFolder, "BuildEnvironment.json"), File.ReadAllText(Path.Combine(originalFolder, "BuildEnvironment.json")));

            var fxFolder = @"C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319";
            var setupCmd = File.ReadAllText(Path.Combine(buildFolder, "CommandPrompt.bat"));
            setupCmd.Should().Contain(fxFolder);

            File.ReadAllText(Path.Combine(buildFolder, "Demo.Web\\Demo.Web.csproj")).Should().Contain("349c5851-65df-11da-9384-00065b846f21", "ProjectTypeGuids should not be replaced");

            var originalGuids = FindGuids(originalFolder);
            var newGuids = FindGuids(buildFolder);
            newGuids.Count.Should().BeGreaterThan(1);

            foreach (var originalGuid in originalGuids)
                if (newGuids.Contains(originalGuid))
                    Assert.Fail("GUID {0} was found in both the original project and the generated project", originalGuid);

            var msBuild = Path.Combine(fxFolder, "msbuild.exe");
            RunVerify(msBuild, "Demo.proj", buildFolder);

            // Assert DB?  NUnit logs?

            RunVerify(msBuild, "Demo.proj /t:clean", buildFolder);
            DeleteFolder(buildFolder, 5);
        }

        private IList<string> FindGuids(string folder)
        {
            var guids = new List<string>();

            var projectFiles = Directory.EnumerateFiles(folder, "*.csproj", SearchOption.AllDirectories);
            var slnFiles = Directory.EnumerateFiles(folder, "*.sln", SearchOption.AllDirectories);
            var files = projectFiles.Union(slnFiles);

            foreach (var file in files)
            {
                var lines = File.ReadAllLines(file);
                foreach (var line in lines)
                    foreach (Match match in Generate.GuidSearch.Matches(line))
                        if (match.Success && !guids.Contains(match.Value) && !Generate.GuidsToIgnore.Contains(match.Value.ToUpper()))
                            guids.Add(match.Value);
            }

            return guids;
        }

        private void CopyFolder(string sourceFolder, string destinationFolder)
        {
            foreach (var dir in Directory.GetDirectories(sourceFolder, "*", SearchOption.AllDirectories))
                Directory.CreateDirectory(dir.Replace(sourceFolder, destinationFolder));

            foreach (var file in Directory.GetFiles(sourceFolder, "*.*", SearchOption.AllDirectories))
                File.Copy(file, file.Replace(sourceFolder, destinationFolder), true);
        }

        private void RunVerify(string fileName, string arguments, string folder)
        {
            var result = Run.Program(fileName, arguments, folder);

            if (result.ExitCode != 0)
            {
                foreach (var outputLine in result.Output)
                    Console.WriteLine(outputLine);

                foreach (var errorLine in result.Errors)
                    Console.Error.WriteLine(errorLine);

                var failMessage = string.Format("Unexpected ExitCode {0} running {1} {2} in {3}", result.ExitCode, fileName, arguments, folder);
                Console.WriteLine(failMessage);
                Assert.Fail(failMessage);
            }
        }

        private void Unzip(byte[] zipBytes, string folder)
        {
            if (Directory.Exists(folder))
                DeleteFolder(folder, 5);

            var buffer = new byte[4096];
            using (var ms = new MemoryStream(zipBytes))
            using (var zipFile = new ZipFile(ms))
                foreach (ZipEntry zipEntry in zipFile)
                {
                    var outFile = Path.Combine(folder, zipEntry.Name);
                    Directory.CreateDirectory(Path.GetDirectoryName(outFile));

                    using (var streamWriter = File.Create(outFile))
                        StreamUtils.Copy(zipFile.GetInputStream(zipEntry), streamWriter, buffer);
                }
        }

        private static void DeleteFolder(string path, int retryCount)
        {
            try
            {
                Directory.Delete(path, true);
            }
            catch
            {
                // *sigh* http://stackoverflow.com/questions/329355/cannot-delete-directory-with-directory-deletepath-true#comment11564214_329502

                if (retryCount <= 0)
                    throw;

                Thread.Sleep(3);
                DeleteFolder(path, retryCount - 1);
            }
        }
    }
}
