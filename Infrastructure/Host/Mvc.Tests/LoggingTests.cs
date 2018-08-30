using System;
using System.IO;
using FluentAssertions;
using NUnit.Framework;

namespace Lucid.Infrastructure.Host.Mvc.Tests
{
    [TestFixture]
    public class GenerateTests
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

            var file = Logging.PrepareConfigFile();

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

            var file = Logging.PrepareConfigFile();

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

            var file = Logging.PrepareConfigFile();

            File.ReadAllText(file).Should().Be("<nlog>newer</nlog>");
        }

        [Test]
        public void MultipleChanges_AreHandled()
        {
            File.WriteAllText("nlog.config", "<nlog>newer</nlog>");
            Logging.PrepareConfigFile();

            File.WriteAllText("nlog.config", "<nlog>newer</nlog>");
            Logging.PrepareConfigFile();
        }
    }
}
