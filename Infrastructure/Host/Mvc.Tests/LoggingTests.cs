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
        public void ConfigFileIsCreatedIfItDoesNotExist()
        {
            File.WriteAllText("nlog.config", "<xml/>");

            Logging.Configure();

            File.Exists("../nlog.mvc.config").Should().BeTrue("config should be created (in parent of app folder) if it does not already exist");
        }
    }
}
