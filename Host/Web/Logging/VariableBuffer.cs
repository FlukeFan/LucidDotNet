using System;
using NLog.Common;
using NLog.Targets;
using NLog.Targets.Wrappers;

namespace Lucid.Host.Web.Logging
{
    /// <summary> Custom NLog buffer to batch emails in increasing batch sizes </summary>
    [Target("VariableBuffer", IsWrapper = true)]
    public class VariableBuffer : BufferingTargetWrapper
    {
        private DateTime    _lastWrite;
        private int         _waitFactor;

        public VariableBuffer()
        {
            _lastWrite = DateTime.MinValue;
            _waitFactor = 1;
            BufferSize = 100000;
            SlidingTimeout = false;
        }

        protected override void Write(AsyncLogEventInfo logEvent)
        {
            const int maxMins = 100 * 60 * 1000;
            const int reduceTimeoutMins = 30;

            var now = DateTime.UtcNow;

            if (FactorTimeout() < maxMins)
                _waitFactor = _waitFactor * 2;

            var decreaseThreshold = _lastWrite + TimeSpan.FromMinutes(reduceTimeoutMins);

            while (_waitFactor > 1 && now > decreaseThreshold)
            {
                _waitFactor = _waitFactor / 2;
                decreaseThreshold += TimeSpan.FromMinutes(reduceTimeoutMins);
            }

            FlushTimeout = FactorTimeout();
            _lastWrite = now;

            InternalLogger.Trace($"VariableBuffer: FlushTimeout={FlushTimeout}");

            base.Write(logEvent);
        }

        private int FactorTimeout()
        {
            var timeout = _waitFactor * 1000;
            return timeout;
        }
    }
}
