using System;
using System.Collections.Generic;
using System.IO;
using FluentAssertions;
using Lucid.Infrastructure.Host.Mvc.Logging;
using NLog.Web;
using NUnit.Framework;

namespace Lucid.Infrastructure.Host.Mvc.Tests.Logging
{
    [TestFixture]
    public class LogConfigTests
    {
        [SetUp]
        public void SetUp()
        {
            var testFolder = "test";
            var testSubFolder = "test//app";

            if (Directory.Exists(testFolder))
                Directory.Delete(testFolder, true);

            Directory.CreateDirectory(testFolder);
            Directory.CreateDirectory(testSubFolder);
            Environment.CurrentDirectory = testSubFolder;
        }

        [Test]
        public void WhenConfigDoesNotExists_ConfigFileIsCreated()
        {
            File.WriteAllText("nlog.config", "<nlog/>");

            var file = LogConfig.PrepareConfigFile();

            Path.IsPathFullyQualified(file).Should().BeTrue($"{file} should be fully qualified");
            File.Exists(file).Should().BeTrue("config should be created (in parent of app folder) if it does not already exist");
        }

        [Test]
        public void WhenTargetConfigIsNewer_ItIsNotOverwritten()
        {
            Directory.CreateDirectory("../logs.config");
            File.WriteAllText("../logs.config/nlog.mvc.config", "<nlog>newer</nlog>");
            File.WriteAllText("nlog.config", "<nlog>older</nlog>");

            var old = DateTime.UtcNow - TimeSpan.FromHours(3);
            File.SetLastWriteTimeUtc("nlog.config", old);

            var file = LogConfig.PrepareConfigFile();

            File.ReadAllText(file).Should().Be("<nlog>newer</nlog>");
        }

        [Test]
        public void WhenTargetConfigIsOlder_ItIsOverwritten()
        {
            Directory.CreateDirectory("../logs.config");
            File.WriteAllText("../logs.config/nlog.mvc.config", "<nlog>older</nlog>");
            File.WriteAllText("nlog.config", "<nlog>newer</nlog>");

            var old = DateTime.UtcNow - TimeSpan.FromHours(3);
            File.SetLastWriteTimeUtc("../logs.config/nlog.mvc.config", old);

            var file = LogConfig.PrepareConfigFile();

            File.ReadAllText(file).Should().EndWith("<nlog>newer</nlog>");
        }

        [Test]
        public void MultipleChanges_AreHandled()
        {
            File.WriteAllText("nlog.config", "<nlog>newer</nlog>");
            LogConfig.PrepareConfigFile();

            File.WriteAllText("nlog.config", "<nlog>newer</nlog>");
            LogConfig.PrepareConfigFile();
        }

        [Test]
        public void WhenConfigUpdated_VariablesAreRetained()
        {
            Directory.CreateDirectory("../logs.config");

            File.WriteAllText("../logs.config/nlog.mvc.config", "<nlog xmlns=\"http://www.nlog-project.org/schemas/NLog.xsd\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">"
                + "<variable name=\"name2\" value=\"server_secret\" />"
                + "</nlog>");

            File.WriteAllText("nlog.config", "<nlog xmlns=\"http://www.nlog-project.org/schemas/NLog.xsd\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">"
                + "<variable name=\"name1\" value=\"value1\" />"
                + "<variable name=\"name2\" value=\"value2\" />"
                + "<variable name=\"name3\" value=\"value3\" />"
                + "</nlog>");


            var old = DateTime.UtcNow - TimeSpan.FromHours(3);
            File.SetLastWriteTimeUtc("../logs.config/nlog.mvc.config", old);

            var file = LogConfig.PrepareConfigFile();

            var values = LogConfig.ReadVariables("../logs.config/nlog.mvc.config");

            values.Should().BeEquivalentTo(new Dictionary<string, string>()
            {
                { "name1", "value1" },
                { "name2", "server_secret" },
                { "name3", "value3" },
            });
        }

        [Test]
        public void RealLoggingFileIsValid()
        {
            var projPath = Path.GetFullPath(".");

            while (!File.Exists(Path.Combine(projPath, "Lucid.Infrastructure.Host.Mvc.Tests.csproj")))
                projPath = Directory.GetParent(projPath).FullName;

            var realConfig = Path.Combine(projPath, "../Mvc/nlog.config");
            NLogBuilder.ConfigureNLog(realConfig);
        }
    }
}
