using System.IO;
using FluentAssertions;
using NUnit.Framework;

namespace Lucid.Database.Tests
{

    [TestFixture]
    public class SettingsTests
    {
        private const string SettingsFileName = "test.settings";

        [Test]
        public void ShouldCreateFileIfItDoesNotExist()
        {
            if (File.Exists(SettingsFileName))
                File.Delete(SettingsFileName);

            Settings.Init(SettingsFileName,
                new Settings1(),
                new Settings2());

            File.Exists(SettingsFileName).Should().BeTrue();

            var storedSettings = File.ReadAllText(SettingsFileName);
            storedSettings.Should().Contain("default S1 value");
            storedSettings.Should().Contain("same name, different value");
        }

        [Test]
        public void ShouldAllowDefaultSettings()
        {
            if (File.Exists(SettingsFileName))
                File.Delete(SettingsFileName);

            var s1 = new Settings1();
            var s2 = new Settings2();

            Settings.Init(SettingsFileName, s1, s2);

            s1.S1.Should().Be("default S1 value");
            s1.S2.Should().BeNull();
            s2.S1.Should().Be("same name, different value");
        }

        [Test]
        public void ShouldNotOverriteExistingSettings()
        {
            File.WriteAllText(SettingsFileName, "<xml><add name='Settings2._s1' value='another value' /></xml>");

            var s2 = new Settings2();

            Settings.Init(SettingsFileName, s2);

            s2.S1.Should().Be("another value");
        }

        [Test]
        public void ShouldNotUpdateIdenticalSettings()
        {
            if (File.Exists(SettingsFileName))
                File.Delete(SettingsFileName);

            Settings.Init(SettingsFileName,
                new Settings1(),
                new Settings2());

            var existingModifiedTime = new FileInfo(SettingsFileName).LastWriteTime;

            Settings.Init(SettingsFileName, new Settings1());

            new FileInfo(SettingsFileName).LastWriteTime.Should().Be(existingModifiedTime, "should not write file if nothing changed");

            Settings.Init(SettingsFileName, new Settings2());

            new FileInfo(SettingsFileName).LastWriteTime.Should().Be(existingModifiedTime, "should not write file if nothing changed");
        }

        public class Settings1
        {
            public string S1 = "default S1 value";
            public string S2;
        }

        public class Settings2
        {
            private string _s1 = "same name, different value";

            public string S1 { get { return _s1; } }
        }
    }
}
