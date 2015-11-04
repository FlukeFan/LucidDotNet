using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Hosting;
using Lucid.Web.Testing.Http;

namespace Lucid.Web.Testing.Hosting
{
    // borrowed from an excellent post on testing asp.net:
    // http://blog.stevensanderson.com/2009/06/11/integration-testing-your-aspnet-mvc-application/
    public class AspNetTestHost : IDisposable
    {
        private bool            _disposed;
        private AppDomainProxy  _appDomainProxy;
        private List<Action>    _deleteTestBinaries = new List<Action>();

        public static AspNetTestHost Current { get; protected set; }

        public string PhysicalDirectory { get; protected set; }

        public AspNetTestHost(string physicalDirectory, string virtualDirectory = "/")
        {
            PhysicalDirectory = physicalDirectory;
            CopyTestBinaries(physicalDirectory);
            _appDomainProxy = (AppDomainProxy)ApplicationHost.CreateApplicationHost(typeof(AppDomainProxy), virtualDirectory, physicalDirectory);
        }

        public static AspNetTestHost For(string physicalDirectory, string virtualDirectory = "/")
        {
            if (Current == null || Current.PhysicalDirectory != physicalDirectory)
            {
                using (Current) { }
                Current = new AspNetTestHost(physicalDirectory, virtualDirectory);
            }

            return Current;
        }

        private void CurrentDomain_DomainUnload(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public void Test(Action<SimulatedHttpClient> testAction)
        {
            var serializableDelegate = new SerializableDelegate<Action<SimulatedHttpClient>>(testAction);
            _appDomainProxy.RunCodeInAppDomain(serializableDelegate, new SimulatedHttpClient());
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~AspNetTestHost()
        {
            Dispose(false);
        }

        private void Dispose(bool isDisposing)
        {
            if (_disposed)
                return;

            if (isDisposing)
            {
                // dispose aggregated managed resources
            }

            if (_appDomainProxy != null)
            {
                _appDomainProxy.RunCodeInAppDomain(() => HttpRuntime.UnloadAppDomain());
                _appDomainProxy = null;
            }

            DeleteTestBinaries();

            _disposed = true;
        }

        private void CopyTestBinaries(string webDir)
        {
            var webBinDir = Path.Combine(webDir, "bin");
            var testBinDir = AppDomain.CurrentDomain.BaseDirectory;

            foreach (var srcFile in Directory.GetFiles(testBinDir))
            {
                var fileName = Path.GetFileName(srcFile);
                var destFile = Path.Combine(webBinDir, fileName);

                if (!File.Exists(destFile))
                {
                    _deleteTestBinaries.Add(() => File.Delete(destFile));
                    File.Copy(srcFile, destFile);
                }
            }

            AppDomain.CurrentDomain.DomainUnload += (s, e) => Dispose();
        }

        private void DeleteTestBinaries()
        {
            _deleteTestBinaries.ForEach(deleteFile => deleteFile());
            _deleteTestBinaries.Clear();
        }
    }
}
