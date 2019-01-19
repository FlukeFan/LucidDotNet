using FluentMigrator;
using Lucid.Infrastructure.Lib.Domain.SqlServer;

namespace Lucid.Modules.Account.DbMigrations
{
    [Maintenance(MigrationStage.BeforeAll)]
    public class BeforeAll : SqlServerMigration
    {
        public BeforeAll(DbQuery dbQuery) : base(dbQuery) { }

        public override void Up()
        {
            var versionsInfo = GetVersionsInfo();
            RemoveFromVersionInfoTable<V001.V000.Script000>(versionsInfo);
        }
    }
}
