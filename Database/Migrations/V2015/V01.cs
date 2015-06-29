using Lucid.Database.Schema;

namespace Lucid.Database.Migrations.V2015
{
    public class V01 : LucidMigration
    {
        public override void Up()
        {
            CreateUserTable();
        }

        private void CreateUserTable()
        {
            var table = Db.Table_User;

            Create.Table(table.Name)
                .LucidPrimaryKey(table.Name, table.Column_Id)
                .WithColumn(table.Column_Email).AsAnsiString().NotNullable();
        }
    }
}
