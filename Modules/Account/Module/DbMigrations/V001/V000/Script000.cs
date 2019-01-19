using Lucid.Infrastructure.Lib.Domain.SqlServer;

namespace Lucid.Modules.Account.DbMigrations.V001.V000
{
    [MigrationOrder(major: 1, minor: 0, script: 0)]
    public class Script000 : SqlServerMigration
    {
        public Script000(DbQuery dbQuery) : base(dbQuery) { }

        public override void Up()
        {
            if (Schema.Table("User").Exists())
                Delete.Table("User");

            Create.Table("User")
                .LucidPrimaryKey("User", "Id")
                .WithColumn("Name").AsAnsiString().NotNullable()
                .WithColumn("LastLoggedIn").AsDateTime().NotNullable();
        }
    }
}
