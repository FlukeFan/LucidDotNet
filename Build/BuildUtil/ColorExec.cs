using System;
using System.Diagnostics;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Build.BuildUtil
{
    public class ColorExec : Task
    {
        [Required]
        public string Command           { get; set; }
        public string WorkingDirectory  { get; set; }

        public override bool Execute()
        {
            using (var process = new Process())
            {
                var firstSpace = Command.IndexOf(' ');

                if (firstSpace > 0)
                {
                    process.StartInfo.FileName = Command.Substring(0, firstSpace);
                    process.StartInfo.Arguments = Command.Substring(firstSpace + 1, Command.Length - firstSpace - 1);
                }
                else
                {
                    process.StartInfo.FileName = Command;
                }

                if (!String.IsNullOrWhiteSpace(WorkingDirectory))
                    process.StartInfo.WorkingDirectory = WorkingDirectory;

                var redirectOutput = BuildEngine2.IsRunningMultipleNodes;

                if (redirectOutput)
                {
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.RedirectStandardError = true;
                    process.OutputDataReceived += (sender, eventArgs) => Log.LogMessage(MessageImportance.High, eventArgs.Data);
                    process.ErrorDataReceived += (sender, eventArgs) => Log.LogError(eventArgs.Data);
                }

                process.StartInfo.UseShellExecute = false;

                Log.LogMessage(MessageImportance.High, $"Starting {process.StartInfo.FileName} {process.StartInfo.Arguments} (in {process.StartInfo.WorkingDirectory}) redirectOutput={redirectOutput}");

                process.Start();

                if (redirectOutput)
                {
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();
                }

                process.WaitForExit();
                return process.ExitCode == 0;
            }
        }
    }
}
