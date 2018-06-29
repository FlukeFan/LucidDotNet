using FluentAssertions;
using NUnit.Framework;
using System;
using System.IO;
using System.IO.Compression;

namespace Lucid.ProjectCreation.__Tests
{
    [TestFixture]
    public class GenerateProjectTests
    {
        private static readonly Func<GenerateProject> _validCommand = () => new GenerateProject { Name = "NewProj_1" };

        [Test]
        public void Execute()
        {
            var cmd = _validCommand();

            var bytes = cmd.Execute();

            var fileCount = 0;

            using (var ms = new MemoryStream(bytes))
            using (var zipFile = new ZipArchive(ms))
                foreach (var zipEntry in zipFile.Entries)
                    fileCount++;

            fileCount.Should().BeGreaterThan(5);
        }

        [Test]
        public void Validation()
        {
            _validCommand().ShouldBeInvalid(c => c.Name = null, "Name cannot be null");
            _validCommand().ShouldBeInvalid(c => c.Name = "", "Name cannot be empty");
            _validCommand().ShouldBeInvalid(c => c.Name = "1", "Name must start with a letter");
            _validCommand().ShouldBeInvalid(c => c.Name = "1abc", "Name must start with a letter");
            _validCommand().ShouldBeInvalid(c => c.Name = "a b", "Name cannot contain spaces");
            _validCommand().ShouldBeInvalid(c => c.Name = " a ", "Name cannot contain spaces");

            foreach (var invalidChar in "!\"£$%^&*()+=-][}{#';/.,~@:?><|\\")
                _validCommand().ShouldBeInvalid(c => c.Name = c.Name.Replace('_', invalidChar), "Name cannot special character: " + invalidChar);
        }
    }

    public static class ValidationExtensions
    {
        public static void ShouldBeInvalid(this GenerateProject command, Action<GenerateProject> invalidate, string reason)
        {
            invalidate(command);
            Assert.Throws<FacadeException>(() => { ExecutableValidator.Validate(command); command.Execute(); }, reason);
        }
    }
}
