using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using FluentAssertions;
using Lucid.Infrastructure.Lib.Facade;
using NUnit.Framework;

namespace Lucid.Modules.ProjectCreation.Tests
{
    [TestFixture]
    public class GenerateProjectTests
    {
        private static readonly Func<GenerateProject> _validCommand = () => new GenerateProject { Name = "NewProj_1" };

        [Test]
        public async Task Execute()
        {
            var cmd = _validCommand();

            var bytes = await cmd.Execute();

            var fileCount = 0;

            using (var ms = new MemoryStream(bytes))
            using (var zipFile = new ZipArchive(ms))
                foreach (var zipEntry in zipFile.Entries)
                    fileCount++;

            fileCount.Should().BeGreaterThan(5);
        }

        [Test]
        [Ignore("Until validation is in place, this is ignored")]
        public void Validation()
        {
            _validCommand().ShouldBeInvalid(c => c.Name = null, "Name cannot be null");
            _validCommand().ShouldBeInvalid(c => c.Name = "", "Name cannot be empty");
            _validCommand().ShouldBeInvalid(c => c.Name = "1", "Name must start with a letter");
            _validCommand().ShouldBeInvalid(c => c.Name = "1abc", "Name must start with a letter");
            _validCommand().ShouldBeInvalid(c => c.Name = "a b", "Name cannot contain spaces");
            _validCommand().ShouldBeInvalid(c => c.Name = " a ", "Name cannot contain spaces");

            foreach (var invalidChar in "!\"£$%^&*()+=-][}{#';/.,~@:?><|\\")
                _validCommand().ShouldBeInvalid(c => c.Name = c.Name.Replace('_', invalidChar), "Name cannot contain special character: " + invalidChar);
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
