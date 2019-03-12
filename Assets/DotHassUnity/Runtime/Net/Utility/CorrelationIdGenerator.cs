using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace DotHass.Unity.Net.Utility
{
    public static class CorrelationIdGenerator
    {
        private static int _lastId = 0;

        public static int GetNextId() => Interlocked.Increment(ref _lastId);
    }
}
