using System;
using FluentMigrator;
using FluentMigrator.Builders.Create.Table;

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
        public override void Down() { throw new NotImplementedException(); }
    }

    public static class CreateTableExtensions
    {
        public static ICreateTableColumnOptionOrWithColumnSyntax LucidPrimaryKey(this ICreateTableWithColumnOrSchemaOrDescriptionSyntax builder, string tableName, string columnName)
        {
            return builder.WithColumn(columnName).AsInt32().PrimaryKey("PK_" + tableName + "_" + columnName).Identity();
        }
    }

    [Maintenance(MigrationStage.BeforeAll)]
    [Migration(0)]
    public class BeforeAll : Migration
    {
        public override void Up()
        {
            // remove from version table any migrations that are WIP
        }

        public override void Down()
        {
        }
    }
}
