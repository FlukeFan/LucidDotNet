namespace Lucid.Infrastructure.Lib.Domain.SqlServer
{
    public class DbConfigProduction : DbConfig
    {
        protected override bool CanCreateDb()
        {
            // production DB will always already exist
            return false;
        }
    }
}