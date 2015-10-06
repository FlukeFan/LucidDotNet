using FluentMigrator.Builders.Create.Table;

namespace Demo.Database.Migrations
{
    public static class LucidExtensions
    {
        public static ICreateTableColumnOptionOrWithColumnSyntax LucidPrimaryKey(this ICreateTableWithColumnOrSchemaOrDescriptionSyntax builder, string tableName, string columnName)
        {
            return builder.WithColumn(columnName).AsInt32().PrimaryKey("PK_" + tableName + "_" + columnName).Identity();
        }
    }
}
