using Lucid.Infrastructure.Lib.Domain.SqlServer;

namespace Lucid.Modules.AppFactory.Design.DbMigrations.V001.V000
{
    [MigrationOrder(major: 1, minor: 0, script: 0)]
    public class Rev0_CreateBlueprintTable : SqlServerMigration
    {
        public override void Up()
        {
            Create.Table("Blueprint")
                .LucidPrimaryKey("Blueprint", "Id")
                .WithColumn("OwnedByUserId").AsInt32().NotNullable()
                .WithColumn("Name").AsAnsiString().NotNullable();
        }
    }
}
