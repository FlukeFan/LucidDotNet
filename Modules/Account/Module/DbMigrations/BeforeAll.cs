using System;
using System.Collections.Generic;
using System.Linq;
using FluentMigrator;
using FluentMigrator.Runner;
using Lucid.Infrastructure.Lib.Domain.SqlServer;
using Microsoft.Extensions.Logging;

namespace Lucid.Modules.Account.DbMigrations
{
    [Maintenance(MigrationStage.BeforeAll)]
    public class BeforeAll : StageMigration
    {
        private ILogger<MigrationRunner> _logger;

        public BeforeAll(ILogger<MigrationRunner> logger)
        {
            _logger = logger;
        }

        public override void Up()
        {
            var versionsInfo = GetVersionsInfo();
            RerunWorkInProgressMigration<V001.V000.Script000>(versionsInfo);
        }

        protected void RerunWorkInProgressMigration<T>(ExistingVersionsInfo versionsInfo)
        {
            var migrationAttribute = (MigrationAttribute)typeof(T).GetCustomAttributes(true).SingleOrDefault(a => typeof(MigrationAttribute).IsAssignableFrom(a.GetType()));
            var version = migrationAttribute.Version;

            if (!versionsInfo.Versions.Contains(version))
                return;

            Execute.Sql($"UPDATE {SchemaVersionMetadata.DefaultTableName} SET {SchemaVersionMetadata.DefaultColumnName} = {versionsInfo.RerunIndex--} WHERE {SchemaVersionMetadata.DefaultColumnName} = {version}");
        }

        protected ExistingVersionsInfo GetVersionsInfo()
        {
            var versionsInfo = new ExistingVersionsInfo();

            if (Schema.Schema("Account").Table("VersionInfo").Exists())
                Execute.WithConnection((cn, tx) =>
                {
                    _logger.LogInformation($"Found versions table");
                    var cmd = cn.CreateCommand();
                    cmd.Transaction = tx;
                    cmd.CommandText = $"SELECT {SchemaVersionMetadata.DefaultColumnName} FROM {SchemaVersionMetadata.DefaultTableName}";

                    using (var reader = cmd.ExecuteReader())
                        while (reader.Read())
                            versionsInfo.Versions.Add(reader.GetInt64(0));
                });

            versionsInfo.RerunIndex = versionsInfo.Versions.Count > 0
                ? Math.Min(versionsInfo.Versions.Min(), -1)
                : -1;

            _logger.LogInformation($"versions: {string.Join(", ", versionsInfo.Versions)}");

            return versionsInfo;
        }

        protected class ExistingVersionsInfo
        {
            public IList<long>  Versions    = new List<long>();
            public long         RerunIndex;
        }
    }
}
