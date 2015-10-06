﻿using System;
using System.IO;
using Newtonsoft.Json;

namespace Demo.Database.Tests
{
    public class BuildEnvironment
    {
        public string MasterConnection;
        public string DemoConnection;

        public static BuildEnvironment Load()
        {
            var folder = Environment.CurrentDirectory;
            var searchFile = "BuildEnvironment.json";

            while (!File.Exists(Path.Combine(folder, searchFile)))
            {
                var dir = new DirectoryInfo(folder);

                if (dir.Parent == null)
                    throw new Exception("Could not find file " + searchFile);

                folder = dir.Parent.FullName;
            }

            var buildEnvironmentFile = Path.Combine(folder, searchFile);
            var json = File.ReadAllText(buildEnvironmentFile);
            var buildEnvironment = JsonConvert.DeserializeObject<BuildEnvironment>(json);

            return buildEnvironment;
        }
    }
}
