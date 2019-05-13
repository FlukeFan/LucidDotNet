using System;
using System.Threading;

namespace Lucid.Lib.Util
{
    public class GlobalLock : IDisposable
    {
        private Semaphore _semaphore;

        public GlobalLock(string identifier) : this(identifier, TimeSpan.FromMinutes(3)) { }

        public GlobalLock(string identifier, TimeSpan timeout)
        {
            bool createdNew_notUsed;
            var semaphoreName = $"Global\\{identifier}";
            _semaphore = new Semaphore(1, 1, semaphoreName, out createdNew_notUsed);
            _semaphore.WaitOne(timeout);
        }

        public void Dispose()
        {
            using (_semaphore)
                _semaphore.Release();
        }
    }
}
