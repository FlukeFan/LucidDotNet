using System;
using System.Collections.Generic;

namespace Lucid.Database.Migrations
{
    public static class DeployedMigrations
    {
        public static IDictionary<Type, string> Hashes = new Dictionary<Type, string>();

        static DeployedMigrations()
        {
            Hashes.Add(typeof(Y2016.M01.V01), "B0C8A4259C22EB970217DEE2E9A82BA5");
            Hashes.Add(typeof(Y2016.M11.V01), "6FC894EA5FFEF8B3B10AEB3CD99493F4");
        }
    }
}
