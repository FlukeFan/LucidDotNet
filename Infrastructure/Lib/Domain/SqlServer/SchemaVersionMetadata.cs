using FluentMigrator.Runner.VersionTableInfo;

namespace Lucid.Infrastructure.Lib.Domain.SqlServer
{
    public class SchemaVersionMetadata : IVersionTableMetaData
    {
        public const string DefaultTableName                = "VersionInfo";
        public const string DefaultColumnName               = "Version";
        public const string DefaultDescriptionColumnName    = "Description";

        public SchemaVersionMetadata(string schemaName)
        {
            SchemaName = schemaName;
        }

        public bool     OwnsSchema              => true;
        public string   ColumnName              => DefaultColumnName;
        public string   SchemaName              { get; }
        public string   TableName               => DefaultTableName;
        public string   UniqueIndexName         => "UC_Version";
        public string   AppliedOnColumnName     => "AppliedOn";
        public string   DescriptionColumnName   => DefaultDescriptionColumnName;
        public object   ApplicationContext      { get; set; }
    }
}
