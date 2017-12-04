using FluentMigrator;

namespace Lucid.Database.Migrations
{
    public class MigrationOrderAttribute : MigrationAttribute
    {
        public MigrationOrderAttribute(int major, int minor = 0, int build = 0, int revision = 0) : base(VersionUtil.VersionFor(major, minor, build, revision))
        {
        }
    }
}
