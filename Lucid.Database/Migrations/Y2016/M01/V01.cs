using FluentMigrator;

namespace Lucid.Database.Migrations.Y2016.M01
{
    [Migration(20160101)]
    public class V01 : LucidMigration
    {
        public override void Up()
        {
            CreateUserTable();
            CreateDesignTable();
        }

        private void CreateUserTable()
        {
            Create.Table("User")
                .LucidPrimaryKey("User", "Id")
                .WithColumn("Email").AsAnsiString().NotNullable()
                .WithColumn("LastLoggedIn").AsDateTime().NotNullable();
        }

        private void CreateDesignTable()
        {

            Create.Table("Design")
                .LucidPrimaryKey("Design", "Id")
                .WithColumn("Name").AsAnsiString().NotNullable();
        }
    }
}
