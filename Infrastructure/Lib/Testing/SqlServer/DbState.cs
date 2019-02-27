using System;

namespace Lucid.Infrastructure.Lib.Testing.SqlServer
{
    public class DbState
    {
        private bool _needsCleaned;

        public DbState()
        {
            MarkDirty();
        }

        public void MarkDirty()
        {
            _needsCleaned = true;
        }

        public void Clean(Action clean)
        {
            if (_needsCleaned)
            {
                clean();
                _needsCleaned = false;
            }
        }
    }
}
