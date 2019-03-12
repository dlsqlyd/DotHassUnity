using DotHass.Unity.Net.Abstractions;
using DotHass.Unity.Net.Message;
using DotHass.Unity.Net.Udp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace DotHass.Unity.Net.Kcp
{

    /// <summary>
    /// 测试：
    /// 1.服务器未开启就连接的时候。read出现错误。后者虽然连接失败但不会出现错误
    /// 2.主机关闭的时候。read出现错误
    /// 3.超时后，read出现错误
    /// </summary>
    class KcpSocketChannel : UdpSocketChannel
    {
        public KcpSocketChannel(ChannelOptions options, KcpOptions kcpOptions) : base(options)
        {
            this.Pipeline = new KcpChannelPipeline(this, kcpOptions);
        }
    }
}
