using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using FluentAssertions;
using Lucid.Infrastructure.Lib.Testing.Validation;
using NUnit.Framework;

namespace Lucid.Modules.ProjectCreation.Tests
{
    [TestFixture]
    public class GenerateProjectCommandTests
    {
        private static readonly Func<GenerateProjectCommand> _validCommand = Agreements.GenerateProject.Executable;

        [Test]
        public async Task Execute()
        {
            var cmd = _validCommand();

            var bytes = await cmd.ExecuteAsync();

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
                _validCommand().ShouldBeInvalid(c => c.Name = c.Name.Replace('_', invalidChar), "Name cannot contain special character: " + invalidChar);
        }
    }
}
