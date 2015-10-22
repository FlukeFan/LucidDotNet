using Demo.Database.Schema;

namespace Demo.Database.Migrations
{
    public class V01 : DemoMigration
    {
        public override void Up()
        {
            CreateUserTable();
            CreateDesignTable();
        }

        private void CreateUserTable()
        {
            var table = Db.Table_User;

            Create.Table(table.Name)
                .DemoPrimaryKey(table.Name, table.Column_Id)
                .WithColumn(table.Column_Email).AsAnsiString().NotNullable()
                .WithColumn(table.Column_LastLoggedIn).AsDateTime().NotNullable();
        }

        private void CreateDesignTable()
        {
            var table = Db.Table_Design;

            Create.Table(table.Name)
                .DemoPrimaryKey(table.Name, table.Column_Id)
                .WithColumn(table.Column_Name).AsAnsiString().NotNullable();
        }
    }
}
