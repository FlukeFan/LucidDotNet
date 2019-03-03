using System;
using System.Collections.Concurrent;

namespace Lucid.Infrastructure.Lib.Util
{
    public static class ProcessLocks
    {
        // these locks will be release only when the process exits
        private static readonly ConcurrentDictionary<string, GlobalLock> _globalLocks = new ConcurrentDictionary<string, GlobalLock>();

        static ProcessLocks()
        {
            AppDomain.CurrentDomain.ProcessExit += (s, e) => ReleaseAll();
        }

        public static void Add(string identifier)
        {
            _globalLocks.GetOrAdd(identifier, id => new GlobalLock(id));
        }

        public static void AddDb(string dbName)
        {
            Add($"LucidDb{dbName}");
        }

        public static void ReleaseAll()
        {
            foreach (var kvp in _globalLocks)
            {
                using (kvp.Value)
                {
                    GlobalLock unused;
                    _globalLocks.TryRemove(kvp.Key, out unused);
                }
            }
        }
    }
}
