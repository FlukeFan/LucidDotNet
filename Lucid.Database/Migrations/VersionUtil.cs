using System;
using System.Linq;
using FluentMigrator;

namespace Lucid.Database.Migrations
{
    public static class VersionUtil
    {
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
