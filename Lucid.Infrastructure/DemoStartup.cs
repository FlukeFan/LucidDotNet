using Lucid.Database;
using Lucid.Infrastructure.NHibernate;

namespace Lucid.Infrastructure
{
    public static class DemoStartup
    {
        public static void Init(string connection)
        {
            LucidMigrationRunner.Run(connection);
            DemoNhRepository.Init(connection);
        }
    }
}
