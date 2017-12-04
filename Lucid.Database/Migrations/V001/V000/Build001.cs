namespace Lucid.Database.Migrations.V001.V000
{
    [MigrationOrder(major: 1, minor: 0, build: 1)]
    public class Build001 : LucidMigration
    {
        public override void Up()
        {
            ModifyUserTable();
        }

        private void ModifyUserTable()
        {
            Rename.Column("Email").OnTable("User").To("Name");
        }
    }
}
