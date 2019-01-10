namespace Lucid.Infrastructure.Lib.Domain.SqlServer
{
    public class DbConfigAppVeyor : DbConfig
    {
        protected override string MasterConnectionString()
        {
            return "Server=(local)\\SQL2017;Database=master;User ID=sa;Password=Password12!";
        }
    }
}
