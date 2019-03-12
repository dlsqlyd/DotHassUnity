using HFramework.Net.Utility;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace HFramework.Net.Message
{
    public static class MessageIDCreater
    {
        private static int count = 1;
        public static int GetNewID()
        {
            return Interlocked.Increment(ref MessageIDCreater.count);
        }
    }
}
