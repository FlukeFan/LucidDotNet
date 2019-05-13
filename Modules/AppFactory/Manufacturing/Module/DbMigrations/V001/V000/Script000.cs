using Lucid.Lib.Domain.SqlServer;

namespace Lucid.Modules.AppFactory.Manufacturing.DbMigrations.V001.V000
{
    [MigrationOrder(major: 1, minor: 0, script: 0)]
    public class Rev0_Empty : SqlServerMigration
    {
        public override void Up()
        {
        }
    }
}
