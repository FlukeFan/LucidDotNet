using FluentMigrator.Runner.VersionTableInfo;

namespace Lucid.Lib.Domain.SqlServer
{
    public class SchemaVersionMetadata : IVersionTableMetaData
    {
        public SchemaVersionMetadata(string schemaName)
        {
            SchemaName = schemaName;
        }

        public bool     OwnsSchema              => true;
        public string   ColumnName              => "Version";
        public string   SchemaName              { get; }
        public string   TableName               => "VersionInfo";
        public string   UniqueIndexName         => "UC_Version";
        public string   AppliedOnColumnName     => "AppliedOn";
        public string   DescriptionColumnName   => "Description";
        public object   ApplicationContext      { get; set; }
    }
}
