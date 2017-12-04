using System;
using System.Collections.Generic;

namespace Lucid.Database.Migrations
{
    public static class DeployedMigrations
    {
        public static IDictionary<Type, string> Hashes = new Dictionary<Type, string>();

        static DeployedMigrations()
        {
            Hashes.Add(typeof(V001.V000.Build000), "EDCA3C7FD6D8A06C8B1261193706954A");
            Hashes.Add(typeof(V001.V000.Build001), "1131984F8B4A37EBCFB9C5583642D73B");
        }
    }
}
