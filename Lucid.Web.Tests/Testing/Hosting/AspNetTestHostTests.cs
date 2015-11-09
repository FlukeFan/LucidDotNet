using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Lucid.Web.Testing.Hosting;
using NUnit.Framework;

namespace Lucid.Web.Tests.Testing.Hosting
{
    [TestFixture]
    public class AspNetTestHostTests
    {
        private static readonly string _web     = @"..\..\..\Web";
        private static readonly string _webBin  = _web + @"\bin";

        [Test]
        public void HostCopiesAndDeletesTestBinaries()
        {
            var testAssembly = Path.GetFileName(typeof(AspNetTestHostTests).Assembly.Location);
            var testBinPath = Path.GetFullPath(".");
            var webBinPath = Path.GetFullPath(_webBin);

            File.Exists(Path.Combine(testBinPath, testAssembly)).Should().BeTrue();
            File.Exists(Path.Combine(webBinPath, testAssembly)).Should().BeFalse();
            File.Exists(Path.Combine(webBinPath, AspNetTestHost.RunningFlagFile)).Should().BeFalse();

            using (AspNetTestHost.For(_web))
            {
                File.Exists(Path.Combine(testBinPath, testAssembly)).Should().BeTrue();
                File.Exists(Path.Combine(webBinPath, testAssembly)).Should().BeTrue();
                File.Exists(Path.Combine(webBinPath, AspNetTestHost.RunningFlagFile)).Should().BeTrue();
            }

            File.Exists(Path.Combine(testBinPath, testAssembly)).Should().BeTrue();
            File.Exists(Path.Combine(webBinPath, testAssembly)).Should().BeFalse();
            File.Exists(Path.Combine(webBinPath, AspNetTestHost.RunningFlagFile)).Should().BeFalse();
        }

        [Test]
        public void DetectsUndiposedInstance()
        {
            var webBinPath = Path.GetFullPath(_webBin);
            var flagFile = Path.Combine(webBinPath, AspNetTestHost.RunningFlagFile);
            try
            {
                File.WriteAllText(flagFile, "simulating instance already running");
                Assert.That(() => AspNetTestHost.For(_web), Throws.Exception);
            }
            finally
            {
                File.Delete(flagFile);
            }
        }

        [Test]
        public void MultipleInstancesPrevented()
        {
            Action startTestHost = () => { using (AspNetTestHost.For(_web)) { Thread.Sleep(100); } };

            var startHosts = new Task[]
            {
                Task.Factory.StartNew(startTestHost),
                Task.Factory.StartNew(startTestHost),
            };

            Task.WaitAll(startHosts);
        }
    }
}
