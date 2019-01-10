namespace Lucid.Infrastructure.Lib.Domain.SqlServer
{
    public class DbConfigDocker : DbConfig
    {
        protected override string MasterConnectionString()
        {
            return "Server=localhost,51433;Database=master;User ID=sa;Password=Password12!";
        }
    }
}