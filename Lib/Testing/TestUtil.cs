using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace Lucid.Lib.Testing
{
    public class TestUtil
    {
        public static string ProjectPath()
        {
            var cd = Path.GetFullPath(".");

            while (Directory.GetFiles(cd, "*.csproj").Length == 0)
            {
                var parent = Directory.GetParent(cd)?.FullName;

                if (parent == cd || parent == null)
                    throw new System.Exception($".csproj not found in parent of {Path.GetFullPath(".")}");

                cd = parent;
            }

            return cd;
        }

        public static IConfigurationRoot GetConfig()
        {
            var searchFile = "Host/web.config.xml";
            var cd = Directory.GetCurrentDirectory();
            var configFile = Path.Combine(cd, searchFile);

            while (!File.Exists(configFile))
            {
                var parent = Directory.GetParent(cd)?.FullName;

                if (parent == cd || parent == null)
                    throw new Exception($"{searchFile} not found in parent of {Directory.GetCurrentDirectory()}");

                cd = parent;
                configFile = Path.Combine(cd, searchFile);
            }

            var config = new ConfigurationBuilder()
                .AddXmlFile(configFile)
                .AddEnvironmentVariables()
                .Build();

            return config;
        }
    }
}
