using Lucid.Infrastructure.Lib.Domain.SqlServer;

namespace Lucid.Modules.Security.DbMigrations.V001.V000
{
    [MigrationOrder(major: 1, minor: 0, script: 0)]
    public class Rev0_CreateUserTable : SqlServerMigration
    {
        public override void Up()
        {
            Create.Table("User")
                .LucidPrimaryKey("User", "Id")
                .WithColumn("Name").AsAnsiString().NotNullable()
                .WithColumn("LastLoggedIn").AsDateTime().NotNullable();
        }
    }
}
