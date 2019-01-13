using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Lucid.Infrastructure.Lib.Domain.SqlServer
{
    public class SqlServer
    {
        private static DbConfig _dbConfig;

        public static void Startup(IHostingEnvironment env, IConfiguration config, params Schema[] schemas)
        {
            var isRunningInAppVeyor = config.GetSection("IsRunningInAppVeyor").Value != null;
            var isDevelopmentMachine = !isRunningInAppVeyor && (env.IsDevelopment() || env.IsEnvironment("Testing"));
            var sqlServerSettings = config.GetSection("SqlServer");

            _dbConfig = GetConfig(isDevelopmentMachine, isRunningInAppVeyor);
            _dbConfig.CreateDb(schemas);
        }

        public static DbConfig GetConfig(bool isDevelopmentMachine, bool isAppVeyor)
        {
            if (isAppVeyor)
                return new DbConfigAppVeyor();

            if (isDevelopmentMachine)
                return new DbConfigDocker();

            return new DbConfigProduction();
        }
    }
}
