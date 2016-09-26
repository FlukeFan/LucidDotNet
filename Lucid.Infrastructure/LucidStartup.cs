using Lucid.Database;
using Lucid.Infrastructure.NHibernate;

namespace Lucid.Infrastructure
{
    public static class LucidStartup
    {
        public static void Init(string connection)
        {
            LucidMigrationRunner.Run(connection);
            LucidNhRepository.Init(connection);
        }
    }
}
