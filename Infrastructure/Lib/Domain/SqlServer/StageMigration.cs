using System.Collections.Generic;
using FluentMigrator;

namespace Lucid.Infrastructure.Lib.Domain.SqlServer
{
    public abstract class StageMigration : Migration
    {
        public override void Down()
        {
        }

        protected IList<int> GetVersions()
        {
            var versions = new List<int>();

            if (Schema.Table(SchemaVersionMetadata.DefaultTableName).Exists())
                Execute.WithConnection((cn, tx) =>
                {
                    var cmd = cn.CreateCommand();
                    cmd.Transaction = tx;
                    cmd.CommandText = $"SELECT {SchemaVersionMetadata.DefaultColumnName} FROM {SchemaVersionMetadata.DefaultTableName}";

                    using (var reader = cmd.ExecuteReader())
                        while (reader.Read())
                            versions.Add(reader.GetInt32(0));
                });

            return versions;
        }
    }
}
