﻿using System;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Logging;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace Lucid.Lib.Testing.SqlServer
{
    public class MigrationUtil
    {
        public class NUnitLoggerProvider : ILoggerProvider
        {
            public ILogger CreateLogger(string categoryName) { return new NUnitLogger(); }
            public void Dispose() { }
        }

        public class NUnitLogger : FluentMigratorRunnerLogger
        {
            protected override void WriteHeading(string message)        { WriteLine($"\nFM: {message}\nFM: {new string('-', message.Length)}"); }
            protected override void WriteElapsedTime(TimeSpan timeSpan) { WriteLine($"FM: Elapsed Time = {timeSpan}"); }
            protected override void WriteEmphasize(string message)      { WriteLine($"FM: *** {message} ***"); }
            protected override void WriteEmptySql()                     { WriteLine($"FM: -- no sql --"); }
            protected override void WriteError(Exception exception)     { WriteLine($"FM: Error - {exception}"); }
            protected override void WriteError(string message)          { WriteLine($"FM: Error - {message}"); }
            protected override void WriteSay(string message)            { WriteLine($"FM: {message}"); }
            protected override void WriteSql(string sql)                { WriteLine($"FM: SQL ({sql})"); }

            public NUnitLogger() : base(Console.Out, Console.Error, new FluentMigratorLoggerOptions { ShowSql = true }) { }

            private void WriteLine(string line)
            {
                TestContext.Progress.WriteLine(line);
            }
        }
    }
}
