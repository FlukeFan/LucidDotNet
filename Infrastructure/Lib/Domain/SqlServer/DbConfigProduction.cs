namespace Lucid.Infrastructure.Lib.Domain.SqlServer
{
    public class DbConfigProduction : DbConfig
    {
        public override void CreateDb()
        {
            // do nothing - production DB will already exist
        }
    }
}