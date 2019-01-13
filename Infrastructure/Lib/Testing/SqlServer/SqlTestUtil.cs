using System;
using Lucid.Infrastructure.Lib.Domain.SqlServer;

namespace Lucid.Infrastructure.Lib.Testing.SqlServer
{
    public class SqlTestUtil
    {
        public static void DropAll()
        {
            var dbConfig = Domain.SqlServer.SqlServer.GetConfig(true, Environment.GetEnvironmentVariable("IsRunningInAppVeyor") != null);
            var schema = new Schema { Name = "Account" };
            dbConfig.CreateDb(schema);
        }
    }
}
