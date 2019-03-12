using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HFramework.Net.Abstractions
{

    public interface IChannel
    {
        int ConnectionId { get; set; }
        ChannelOptions Options { get; }
        IChannelPipeline Pipeline { get; set; }

        Task<IChannelPipeline> ConnectAsync();

        IChannel Flush();

        Task CloseAsync();
        void ReConnectAsync();
        bool IsActive();
    }
}
