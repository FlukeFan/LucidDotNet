using System;
using System.Collections.Generic;

namespace Lucid.Database.Migrations
{
    public static class DeployedMigrations
    {
        public static IDictionary<Type, string> Hashes = new Dictionary<Type, string>();

        static DeployedMigrations()
        {
            Hashes.Add(typeof(Y2016.M01.V01), "5BDDDD7C84CF58D2285118EB2AA85D2A");
        }
    }
}
