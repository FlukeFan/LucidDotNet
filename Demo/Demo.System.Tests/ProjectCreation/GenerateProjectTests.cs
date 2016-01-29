using System;
using System.IO;
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

            var buildFolder = Path.Combine(Path.GetTempPath(), "GpTmp");
            Unzip(zipBytes, buildFolder);
            CopyFolder(@"..\..\..\packages", Path.Combine(buildFolder, "packages"));
            File.WriteAllText(Path.Combine(buildFolder, "BuildEnvironment.json"), File.ReadAllText(@"..\..\..\BuildEnvironment.json"));

            var fxFolder = @"C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319";
            var setupCmd = File.ReadAllText(Path.Combine(buildFolder, "CommandPrompt.bat"));
            setupCmd.Should().Contain(fxFolder);

            var msBuild = Path.Combine(fxFolder, "msbuild.exe");
            RunVerify(msBuild, "Demo.proj", buildFolder);

            // Assert DB?  NUnit logs?

            RunVerify(msBuild, "Demo.proj /t:clean", buildFolder);
            DeleteFolder(buildFolder, 5);
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
            Console.WriteLine("Uzipping to: {0}", folder);

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
