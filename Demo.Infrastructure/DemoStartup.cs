using Demo.Database;
using Demo.Infrastructure.NHibernate;

namespace Demo.Infrastructure
{
    public static class DemoStartup
    {
        public static void Init(string connection)
        {
            DemoMigrationRunner.Run(connection);
            DemoNhRepository.Init(connection);
        }
    }
}
