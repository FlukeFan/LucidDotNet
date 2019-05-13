using System;
using FluentMigrator;
using FluentMigrator.Builders.Create.Table;
using FluentMigrator.Builders.Schema;
using FluentMigrator.Builders.Schema.Schema;

namespace Lucid.Lib.Domain.SqlServer
{
    public class MigrationOrderAttribute : MigrationAttribute
    {
        public MigrationOrderAttribute(int major, int minor = 0, int script = 0, int rev = 0) : base(((major * 1000 + minor) * 1000 + script) * 1000 + rev)
        {
        }
    }

    public abstract class SqlServerMigration : Migration
    {
        public SqlServerMigration() : this(null) { }

        public SqlServerMigration(DbQuery dbQuery)
        {
            DbQuery = dbQuery;
        }

        public DbQuery DbQuery { get; }

        public override void Down() { throw new NotImplementedException(); }

        public ISchemaExpressionRoot Db { get { return base.Schema; } }

        public new ISchemaSchemaSyntax Schema { get { return Db.Schema(DbQuery.SchemaName);  } }
    }

    public static class CreateTableExtensions
    {
        public static ICreateTableColumnOptionOrWithColumnSyntax LucidPrimaryKey(this ICreateTableWithColumnOrSchemaOrDescriptionSyntax builder, string tableName, string columnName)
        {
            return builder.WithColumn(columnName).AsInt32().PrimaryKey("PK_" + tableName + "_" + columnName).Identity();
        }
    }
}
