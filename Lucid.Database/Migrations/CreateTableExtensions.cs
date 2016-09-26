using FluentMigrator.Builders.Create.Table;

namespace Lucid.Database.Migrations
{
    public static class CreateTableExtensions
    {
        public static ICreateTableColumnOptionOrWithColumnSyntax LucidPrimaryKey(this ICreateTableWithColumnOrSchemaOrDescriptionSyntax builder, string tableName, string columnName)
        {
            return builder.WithColumn(columnName).AsInt32().PrimaryKey("PK_" + tableName + "_" + columnName).Identity();
        }
    }
}
