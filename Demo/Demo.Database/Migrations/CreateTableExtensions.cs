using FluentMigrator.Builders.Create.Table;

namespace Demo.Database.Migrations
{
    public static class CreateTableExtensions
    {
        public static ICreateTableColumnOptionOrWithColumnSyntax DemoPrimaryKey(this ICreateTableWithColumnOrSchemaOrDescriptionSyntax builder, string tableName, string columnName)
        {
            return builder.WithColumn(columnName).AsInt32().PrimaryKey("PK_" + tableName + "_" + columnName).Identity();
        }
    }
}
