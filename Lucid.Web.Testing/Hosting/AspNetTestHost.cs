using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Web;
using System.Web.Hosting;
using Lucid.Web.Testing.Http;

namespace Lucid.Web.Testing.Hosting
{
    // borrowed from an excellent post on testing asp.net:
    // http://blog.stevensanderson.com/2009/06/11/integration-testing-your-aspnet-mvc-application/
    public class AspNetTestHost : IDisposable
    {
        public const string     RunningFlagFile = "AspNetTestHost.running.txt";

        private bool            _disposed;
        private Semaphore       _enforceSingleInstance;
        private AppDomainProxy  _appDomainProxy;
        private List<Action>    _deleteActions = new List<Action>();

        public string PhysicalDirectory { get; protected set; }

        public AspNetTestHost(string physicalDirectory) : this(physicalDirectory, "/", Timeout.InfiniteTimeSpan) { }

        public AspNetTestHost(string physicalDirectory, string virtualDirectory, TimeSpan timeout)
        {
            PhysicalDirectory = Path.GetFullPath(physicalDirectory);

            var semaphoreName = "Global\\LucidAspNetTestHost" + PhysicalDirectory.GetHashCode();
            _enforceSingleInstance = new Semaphore(1, 1, semaphoreName);

            try
            {
                if (!_enforceSingleInstance.WaitOne(timeout))
                    throw new Exception("Could not obtain semaphore: " + semaphoreName);

                if (!Directory.Exists(PhysicalDirectory))
                    throw new Exception("Could not find directory: " + PhysicalDirectory);

                CopyTestBinaries(PhysicalDirectory);
                _appDomainProxy = (AppDomainProxy)ApplicationHost.CreateApplicationHost(typeof(AppDomainProxy), virtualDirectory, PhysicalDirectory);
            }
            catch
            {
                DeleteTestBinaries();
                using (_enforceSingleInstance)
                    _enforceSingleInstance.Release();
                throw;
            }
        }

        public static AspNetTestHost For(string physicalDirectory)
        {
            return new AspNetTestHost(physicalDirectory);
        }

        public static AspNetTestHost For(string physicalDirectory, string virtualDirectory, TimeSpan timeout)
        {
            return new AspNetTestHost(physicalDirectory, virtualDirectory, timeout);
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

            if (_appDomainProxy != null)
            {
                _appDomainProxy.RunCodeInAppDomain(() => HttpRuntime.UnloadAppDomain());
                _appDomainProxy = null;
            }

            DeleteTestBinaries();

            if (isDisposing && _enforceSingleInstance != null)
                using (_enforceSingleInstance)
                {
                    _enforceSingleInstance.Release();
                    _enforceSingleInstance = null;
                }

            _disposed = true;
        }

        private void CopyTestBinaries(string webDir)
        {
            var webBinDir = Path.Combine(webDir, "bin");
            var testBinDir = AppDomain.CurrentDomain.BaseDirectory;
            var runningFlagFile = Path.Combine(webBinDir, RunningFlagFile);

            if (!Directory.Exists(webBinDir))
                throw new Exception("Could not find bin directory: " + webBinDir);

            if (File.Exists(runningFlagFile))
                throw new Exception("Previous instance of AspNetTestHosts was not cleanly disposed - you might need to clean/rebuild your web folder");

            AppDomain.CurrentDomain.DomainUnload += (s, e) => Dispose();

            File.WriteAllText(runningFlagFile, "AspNetTestHost running");

            foreach (var srcFile in Directory.GetFiles(testBinDir))
            {
                var fileName = Path.GetFileName(srcFile);
                var destFile = Path.Combine(webBinDir, fileName);

                if (!File.Exists(destFile))
                {
                    _deleteActions.Add(() => File.Delete(destFile));
                    File.Copy(srcFile, destFile);
                }
            }

            _deleteActions.Add(() => File.Delete(runningFlagFile));
        }

        private void DeleteTestBinaries()
        {
            _deleteActions.ForEach(deleteAction => deleteAction());
            _deleteActions.Clear();
        }
    }
}
