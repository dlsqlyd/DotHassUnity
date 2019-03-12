using System;

namespace DotHass.Unity.Net.Abstractions
{
    public interface IChannelPipeline
    {
        IChannel Channel { get; set; }
        int ClientID { get; set; }
        string SessionID { get; set; }

        Action<IChannelPipeline> OnHandShake { get; set; }
        Action<IChannelPipeline, IResponseMessage> OnRead { get; set; }
        Action<IChannelPipeline> OnError { get; set; }

        void FireConnectError();

        void FireChannelActive();
        void FireChannelRead(byte[] buffer, int offset, int received);

        void WriteAndFlushAsync(IRequestMessage message);

        byte[] GetOutputBuffer();
    }
}
