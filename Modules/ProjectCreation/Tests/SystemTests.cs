using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Lucid.Infrastructure.Lib.Testing;
using NUnit.Framework;

namespace Lucid.Modules.ProjectCreation.Tests
{
    [TestFixture]
    public class SystemTests
    {
        [SlowTest]
        [Test]
        public async Task Execute()
        {
            var cmd = new GenerateProjectCommand { Name = "ShinyNewProject1" };

            var zipBytes = await cmd.ExecuteAsync();

            //var originalFolder = @"..\..\..";
            var buildFolder = Path.Combine(Path.GetTempPath(), "GpTmp");
            TestContext.Progress.WriteLine($"Building project {cmd.Name} in {buildFolder}");
            Unzip(zipBytes, buildFolder);

            Exec.Cmd("dotnet", "restore build.proj", buildFolder);
            Exec.Cmd("dotnet", "msbuild build.proj /m:1 /p:FilterTest=TestCategory!=Slow", buildFolder);

            //// Assert DB?  NUnit logs?

            Exec.Cmd("dotnet", "clean build.proj", buildFolder);
            Exec.Cmd("dotnet", "build-server shutdown", buildFolder);
            DeleteFolder(buildFolder, 5);
        }

        private void Unzip(byte[] zipBytes, string folder)
        {
            if (Directory.Exists(folder))
                DeleteFolder(folder, 5);

            using (var ms = new MemoryStream(zipBytes))
            using (var zipFile = new ZipArchive(ms))
                foreach (var zipEntry in zipFile.Entries)
                {
                    var outFile = Path.Combine(folder, zipEntry.FullName);
                    Directory.CreateDirectory(Path.GetDirectoryName(outFile));

                    using (var streamWriter = File.Create(outFile))
                    using (var inputStream = zipEntry.Open())
                        inputStream.CopyTo(streamWriter);
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

        public static class Exec
        {
            public static void Cmd(string fileName, string arguments, string workingDirectory)
            {
                using (Process process = new Process())
                {
                    process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    process.StartInfo.CreateNoWindow = true;
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.RedirectStandardError = true;
                    process.StartInfo.FileName = fileName;
                    process.StartInfo.Arguments = arguments;
                    process.StartInfo.WorkingDirectory = workingDirectory;

                    // ensure any environment variables are copied to sub-process (e.g., AppVeyor specific variables)
                    var envVars = Environment.GetEnvironmentVariables();
                    foreach (DictionaryEntry envVar in envVars)
                        if (envVar.Key != null)
                            process.StartInfo.EnvironmentVariables[(string)envVar.Key] = (string)envVar.Value;

                    TestContext.Progress.WriteLine($"Running {process.StartInfo.FileName} {process.StartInfo.Arguments} (in {process.StartInfo.WorkingDirectory})");

                    using (AutoResetEvent outputWaitHandle = new AutoResetEvent(false))
                    using (AutoResetEvent errorWaitHandle = new AutoResetEvent(false))
                    {
                        process.OutputDataReceived += (sender, eventArgs) =>
                        {
                            if (eventArgs.Data == null)
                                outputWaitHandle.Set();
                            else
                                TestContext.Progress.WriteLine($"INFO:  {eventArgs.Data}");
                        };

                        process.ErrorDataReceived += (sender, eventArgs) =>
                        {
                            if (eventArgs.Data == null)
                                outputWaitHandle.Set();
                            else
                                TestContext.Progress.WriteLine($"ERROR: {eventArgs.Data}");
                        };

                        process.Start();
                        process.BeginOutputReadLine();
                        process.BeginErrorReadLine();

                        process.WaitForExit();

                        TestContext.Progress.WriteLine($"Process exited with code {process.ExitCode}");
                        process.ExitCode.Should().Be(0);
                    }
                }
            }
        }
    }
}
