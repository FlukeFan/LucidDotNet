using Demo.Infrastructure.NHibernate;

namespace Demo.Infrastructure
{
    public static class DemoStartup
    {
        public static void Init(string connection)
        {
            DemoNhRepository.Init(connection);
        }
    }
}
