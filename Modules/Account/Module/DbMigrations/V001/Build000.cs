using FluentMigrator;
using Lucid.Infrastructure.Lib.Domain.SqlServer;

namespace Lucid.Modules.Account.DbMigrations.V001
{
    [Migration(001000)]
    public class Build000 : SqlServerMigration
    {
        public override void Up()
        {
            CreateUserTable();
        }

        private void CreateUserTable()
        {
            Create.Table("User")
                .LucidPrimaryKey("User", "Id")
                .WithColumn("Name").AsAnsiString().NotNullable()
                .WithColumn("LastLoggedIn").AsDateTime().NotNullable();
        }
    }
}
