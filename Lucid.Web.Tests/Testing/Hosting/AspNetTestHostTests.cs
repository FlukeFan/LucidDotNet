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
        private static readonly string _web     = @"..\..\..\Lucid.Web.StubApp";
        private static readonly string _webBin  = _web + @"\bin";

        private Semaphore _enforceSingle;

        [SetUp]
        public void SetUp()
        {
            _enforceSingle = new Semaphore(1, 1, "Global\\AspNetTestHostTests");
            _enforceSingle.WaitOne();
        }

        [TearDown]
        public void TearDown()
        {
            using (_enforceSingle)
                _enforceSingle.Release();
        }

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

        [Test]
        public void FolderDoesNotExist_Throws()
        {
            var tmpFolder = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            try
            {
                Directory.CreateDirectory(tmpFolder);
                Directory.CreateDirectory(Path.Combine(tmpFolder, "bin"));

                {
                    var nonExistentFolder = Path.Combine(tmpFolder, "doesNotExist");
                    var folderError = Assert.Throws<Exception>(() => AspNetTestHost.For(nonExistentFolder, "/", TimeSpan.FromSeconds(15)));
                    folderError.Message.Should().Be("Could not find directory: " + nonExistentFolder);
                }

                {
                    var folderWithoutBin = Path.Combine(tmpFolder, "bin");
                    var binError = Assert.Throws<Exception>(() => AspNetTestHost.For(folderWithoutBin, "/", TimeSpan.FromSeconds(15)));
                    binError.Message.Should().Be("Could not find bin directory: " + Path.Combine(folderWithoutBin, "bin"));
                }
            }
            finally
            {
                Directory.Delete(tmpFolder, true);
            }
        }

        public class AssemblyUnloadTest : MarshalByRefObject
        {
            private AspNetTestHost _host;

            public void Go()
            {
                _host = AspNetTestHost.For(_web);
            }

            public void SuppressFinalize()
            {
                GC.SuppressFinalize(_host);
            }
        }

        [Test]
        public void AssemblyUnload_Disposes()
        {
            var type = typeof(AssemblyUnloadTest);
            var webBinPath = Path.GetFullPath(_webBin);
            var flagFile = Path.Combine(webBinPath, AspNetTestHost.RunningFlagFile);

            {
                // test finalizer

                var appDomain = AppDomain.CreateDomain("AssemblyUnloadTest", null, AppDomain.CurrentDomain.SetupInformation);
                var proxy = (AssemblyUnloadTest)appDomain.CreateInstanceAndUnwrap(type.Assembly.FullName, type.FullName);
                proxy.Go();

                File.Exists(flagFile).Should().BeTrue();

                AppDomain.Unload(appDomain);

                File.Exists(flagFile).Should().BeFalse();
            }

            {
                // test dispose gets called on DomainUnload event

                var appDomain = AppDomain.CreateDomain("AssemblyUnloadTest", null, AppDomain.CurrentDomain.SetupInformation);
                var proxy = (AssemblyUnloadTest)appDomain.CreateInstanceAndUnwrap(type.Assembly.FullName, type.FullName);
                proxy.Go();

                File.Exists(flagFile).Should().BeTrue();

                proxy.SuppressFinalize();
                AppDomain.Unload(appDomain);

                File.Exists(flagFile).Should().BeFalse();
            }
        }
    }
}
