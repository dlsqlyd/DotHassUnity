using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotHass.Unity.Net
{
    [Serializable]
    public class ChannelOptions
    {

        public ChannelType Type;

        public string Name ="Game";

        public string IP = "127.0.0.1";

        public int Port = 6666;

        public int MaxFrameLength = 8192;

        public string key = "1xKJagtnC6jwqgj9";
    }

    [Serializable]
    public enum ChannelType
    {
        TCP,
        KCP
    }
}
