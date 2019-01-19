using System;
using System.Collections.Generic;
using System.Linq;
using FluentMigrator;
using FluentMigrator.Builders.Create.Table;
using FluentMigrator.Builders.Schema;
using FluentMigrator.Builders.Schema.Schema;

namespace Lucid.Infrastructure.Lib.Domain.SqlServer
{
    public class MigrationOrderAttribute : MigrationAttribute
    {
        public MigrationOrderAttribute(int major, int minor = 0, int script = 0) : base(((major * 1000 + minor) * 1000 + script) * 1000)
        {
        }
    }

    public abstract class SqlServerMigration : Migration
    {
        public SqlServerMigration(DbQuery dbQuery)
        {
            DbQuery = dbQuery;
        }

        public DbQuery DbQuery { get; }

        public override void Down() { throw new NotImplementedException(); }

        public ISchemaExpressionRoot Db { get { return base.Schema; } }

        public new ISchemaSchemaSyntax Schema { get { return Db.Schema(DbQuery.SchemaName);  } }


        protected void RemoveFromVersionInfoTable<T>(ExistingVersionsInfo versionsInfo)
        {
            var migrationAttribute = (MigrationAttribute)typeof(T).GetCustomAttributes(true).SingleOrDefault(a => typeof(MigrationAttribute).IsAssignableFrom(a.GetType()));
            var version = migrationAttribute.Version;

            if (!versionsInfo.Versions.Contains(version))
                return;

            Execute.Sql($@"
                UPDATE {SchemaVersionMetadata.DefaultTableName}
                SET {SchemaVersionMetadata.DefaultColumnName} = {versionsInfo.RerunIndex--}
                 , {SchemaVersionMetadata.DefaultDescriptionColumnName} = 'Original({version}) ' + {SchemaVersionMetadata.DefaultDescriptionColumnName}
                WHERE {SchemaVersionMetadata.DefaultColumnName} = {version}");
        }

        protected ExistingVersionsInfo GetVersionsInfo()
        {
            var versionsInfo = new ExistingVersionsInfo();

            if (Schema.Table("VersionInfo").Exists())
            {
                var sql = $"SELECT {SchemaVersionMetadata.DefaultColumnName} FROM {SchemaVersionMetadata.DefaultTableName}";
                versionsInfo.Versions = DbQuery.ExecuteList<long>(sql);
            }

            versionsInfo.RerunIndex = versionsInfo.Versions.Count > 0
                ? Math.Min(versionsInfo.Versions.Min(), -1)
                : -1;

            return versionsInfo;
        }

        protected class ExistingVersionsInfo
        {
            public IList<long>  Versions    = new List<long>();
            public long         RerunIndex;
        }
    }

    public static class CreateTableExtensions
    {
        public static ICreateTableColumnOptionOrWithColumnSyntax LucidPrimaryKey(this ICreateTableWithColumnOrSchemaOrDescriptionSyntax builder, string tableName, string columnName)
        {
            return builder.WithColumn(columnName).AsInt32().PrimaryKey("PK_" + tableName + "_" + columnName).Identity();
        }
    }
}
