using System;
using System.Threading;

namespace Lucid.Infrastructure.Lib.Util
{
    public class GlobalLock : IDisposable
    {
        private Semaphore _semaphore;

        public GlobalLock(string identifier)
        {
            bool createdNew_notUsed;
            var semaphoreName = $"Global\\{identifier}";
            _semaphore = new Semaphore(1, 1, semaphoreName, out createdNew_notUsed);
            _semaphore.WaitOne();
        }

        public void Dispose()
        {
            using (_semaphore)
                _semaphore.Release();
        }
    }
}
