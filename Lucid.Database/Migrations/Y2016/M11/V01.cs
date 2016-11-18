using FluentMigrator;

namespace Lucid.Database.Migrations.Y2016.M11
{
    [Migration(20161101)]
    public class V01 : LucidMigration
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
