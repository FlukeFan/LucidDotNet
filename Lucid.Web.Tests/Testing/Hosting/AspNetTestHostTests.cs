using System.IO;
using FluentAssertions;
using Lucid.Web.Testing.Hosting;
using NUnit.Framework;

namespace Lucid.Web.Tests.Testing.Hosting
{
    [TestFixture]
    public class AspNetTestHostTests
    {
        [Test]
        public void HostCopiesAndDeletesTestBinaries()
        {
            var testAssembly = Path.GetFileName(typeof(AspNetTestHostTests).Assembly.Location);
            var testBinPath = Path.GetFullPath(".");
            var webBinPath = Path.GetFullPath(@"..\..\..\Web\bin");

            File.Exists(Path.Combine(testBinPath, testAssembly)).Should().BeTrue();
            File.Exists(Path.Combine(webBinPath, testAssembly)).Should().BeFalse();

            using (AspNetTestHost.For(@"..\..\..\Web"))
            {
                File.Exists(Path.Combine(testBinPath, testAssembly)).Should().BeTrue();
                File.Exists(Path.Combine(webBinPath, testAssembly)).Should().BeTrue();
            }

            File.Exists(Path.Combine(testBinPath, testAssembly)).Should().BeTrue();
            File.Exists(Path.Combine(webBinPath, testAssembly)).Should().BeFalse();
        }
    }
}
