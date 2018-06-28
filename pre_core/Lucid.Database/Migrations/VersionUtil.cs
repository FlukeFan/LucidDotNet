using System;
using System.Linq;
using FluentMigrator;

namespace Lucid.Database.Migrations
{
    public static class VersionUtil
    {
        public static long VersionFor(int major, int minor = 0, int build = 0, int revision = 0)
        {
            return ((major * 1000 + minor) * 1000 + build) * 1000 + revision;
        }

        public static long GetVersion(Type migrationType)
        {
            var migrationAttribute = migrationType.GetCustomAttributes(typeof(MigrationAttribute), false).Cast<MigrationAttribute>().SingleOrDefault();

            if (migrationAttribute == null)
                throw new Exception(string.Format("Could not find MigrationAttribute on {0}", migrationType));

            var version = migrationAttribute.Version;
            return version;
        }
    }
}
