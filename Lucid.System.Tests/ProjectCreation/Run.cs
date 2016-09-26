using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Demo.System.Tests.ProjectCreation
{
    public static class Run
    {
        public static RunResult Program(string fileName, string arguments)
        {
            return Program(fileName, arguments, Environment.CurrentDirectory);
        }

        public static RunResult Program(string fileName, string arguments, string workingDirectory)
        {
            var result = new RunResult();

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

                using (AutoResetEvent outputWaitHandle = new AutoResetEvent(false))
                using (AutoResetEvent errorWaitHandle = new AutoResetEvent(false))
                {
                    process.OutputDataReceived += (sender, eventArgs) =>
                    {
                        if (eventArgs.Data == null)
                            outputWaitHandle.Set();
                        else
                            result.Output.Add(eventArgs.Data);
                    };

                    process.ErrorDataReceived += (sender, eventArgs) =>
                    {
                        if (eventArgs.Data == null)
                            outputWaitHandle.Set();
                        else
                            result.Errors.Add(eventArgs.Data);
                    };

                    process.Start();
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();

                    process.WaitForExit();

                    result.ExitCode = process.ExitCode;
                }
            }

            return result;
        }

        public class RunResult
        {
            public int ExitCode;
            public IList<string> Output = new List<string>();
            public IList<string> Errors = new List<string>();
        }
    }
}
