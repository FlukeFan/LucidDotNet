using System;
using System.Collections.Generic;

namespace Demo.Database.Migrations
{
    public static class DeployedMigrations
    {
        public static IDictionary<Type, string> Hashes = new Dictionary<Type, string>();

        static DeployedMigrations()
        {
            Hashes.Add(typeof(Y2016.M01.V01), "667AEF850D5C19E2E08CAD20E5769EAB");
        }
    }
}
