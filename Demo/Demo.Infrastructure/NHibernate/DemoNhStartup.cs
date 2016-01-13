namespace Demo.Infrastructure.NHibernate
{
    public static class DemoNhStartup
    {
        public static void Init(string connection)
        {
            DemoNhRepository.Init(connection);
        }
    }
}
