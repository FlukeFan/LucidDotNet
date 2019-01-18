using FluentMigrator;
using Lucid.Infrastructure.Lib.Domain.SqlServer;

namespace Lucid.Modules.Account.DbMigrations
{
    [Maintenance(MigrationStage.BeforeAll)]
    public class BeforeAll : StageMigration
    {
        public override void Up()
        {
            var versions = GetVersions();

            if (versions.Count > 0)
                throw new System.Exception($"Found {versions.Count} existing versions - need to check if they need removed");
        }
    }
}
