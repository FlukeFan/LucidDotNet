namespace Lucid.Database.Migrations.V001.V000
{
    [MigrationOrder(major: 1, minor: 0, build: 0)]
    public class Build000 : LucidMigration
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
