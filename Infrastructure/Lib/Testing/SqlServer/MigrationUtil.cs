using System;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Logging;
using FluentMigrator.Runner.VersionTableInfo;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace Lucid.Infrastructure.Lib.Testing.SqlServer
{
    public class MigrationUtil
    {
        public class SchemaVersionTable : IVersionTableMetaData
        {
            private string _schemaName;

            public SchemaVersionTable(string schemaName)
            {
                _schemaName = schemaName;
            }

            public bool OwnsSchema => true;
            public string ColumnName => "Version";
            public string SchemaName => _schemaName;
            public string TableName => "VersionInfo";
            public string UniqueIndexName => "UC_Version";
            public string AppliedOnColumnName => "AppliedOn";
            public string DescriptionColumnName => "Description";
            public object ApplicationContext { get; set; }
        }

        public class NUnitLoggerProvider : ILoggerProvider
        {
            public ILogger CreateLogger(string categoryName)
            {
                return new NUnitLogger();
            }

            public void Dispose()
            {
            }
        }

        public class NUnitLogger : FluentMigratorRunnerLogger
        {
            protected override void WriteHeading(string message) { WriteLine($"Heading: {message}"); }
            protected override void WriteElapsedTime(TimeSpan timeSpan) { WriteLine($"Elapsed Time: {timeSpan}"); }
            protected override void WriteEmphasize(string message) { WriteLine($"Emphasize: {message}"); }
            protected override void WriteEmptySql() { WriteLine($"Empty SQL"); }
            protected override void WriteError(Exception exception) { WriteLine($"Error: {exception}"); }
            protected override void WriteError(string message) { WriteLine($"Error: {message}"); }
            protected override void WriteSay(string message) { WriteLine($"Say: {message}"); }
            protected override void WriteSql(string sql) { WriteLine($"SQL: {sql}"); }

            public NUnitLogger() : base(Console.Out, Console.Error, new FluentMigratorLoggerOptions()) { }

            private void WriteLine(string line)
            {
                TestContext.Progress.WriteLine(line);
            }
        }
    }
}
