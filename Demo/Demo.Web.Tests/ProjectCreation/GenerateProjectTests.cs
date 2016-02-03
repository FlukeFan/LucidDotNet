using System;
using System.IO;
using Demo.Web.ProjectCreation;
using FluentAssertions;
using ICSharpCode.SharpZipLib.Zip;
using NUnit.Framework;

namespace Demo.Web.Tests.ProjectCreation
{
    [TestFixture]
    public class GenerateProjectTests
    {
        private static readonly Func<GenerateProject> _validCmd = () => new GenerateProject { Name = "NewProj1" };

        [Test]
        public void Execute()
        {
            var cmd = _validCmd();

            var bytes = cmd.Execute();

            var buffer = new byte[4096];
            var fileCount = 0;

            using (var ms = new MemoryStream(bytes))
            using (var zipFile = new ZipFile(ms))
                foreach (ZipEntry zipEntry in zipFile)
                    fileCount++;

            fileCount.Should().BeGreaterThan(5);
        }
    }
}
