using Microsoft.Extensions.Configuration;

namespace Lucid.Infrastructure.Lib.Domain.SqlServer
{
    public class SqlServer
    {
        private static DbConfig _dbConfig;

        public static void Startup(bool isDevelopment, IConfiguration config)
        {
            var isRunningInAppVeyor = config.GetSection("IsRunningInAppVeyor").Value != null;
            var sqlServerSettings = config.GetSection("SqlServer");
            _dbConfig = GetConfig(isDevelopment, isRunningInAppVeyor);

            _dbConfig.CreateDb();
        }

        private static DbConfig GetConfig(bool isDevelopment, bool isAppVeyor)
        {
            if (isAppVeyor)
                return new DbConfigAppVeyor();

            if (isDevelopment)
                return new DbConfigDocker();

            return new DbConfigProduction();
        }
    }
}
