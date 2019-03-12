using DotHass.Unity.Net.Abstractions;
using DotHass.Unity.Net.Message;
using System;

namespace DotHass.Unity.Net
{
    public abstract class DefaultChannelPipeline : IChannelPipeline
    {
        public int ClientID { get; set; } = 0;
        public string SessionID { get; set; } = String.Empty;


        public ByteBuffer OutputStream { get; set; }
        public IChannel Channel { get; set; }

        public Action<IChannelPipeline> OnHandShake { get; set; }
        public Action<IChannelPipeline, IResponseMessage> OnRead { get; set; }
        public Action<IChannelPipeline> OnError { get; set; }


        public DefaultChannelPipeline(IChannel channel)
        {
            this.Channel = channel;
            this.OutputStream = ByteBuffer.Allocate(channel.Options.MaxFrameLength);
        }


        public virtual void FireChannelRead(byte[] buffer, int offset, int received)
        {
            if (OnRead != null)
            {
                byte[] bytes = new byte[received];
                Array.Copy(buffer, offset, bytes, 0, bytes.Length);
                this.OnRead(this, MessageFactory.Parse(bytes));
            }
        }


        /// <summary>
        /// 触发连接错误的时候
        /// </summary>
        public virtual void FireConnectError()
        {
            this.ClientID = 0;
            this.SessionID = String.Empty;
            HandlerError();
        }


        private void HandlerError()
        {
            if (OnError != null)
            {
                this.OnError(this);
            }
        }

        public virtual byte[] GetOutputBuffer()
        {
            var bytes = OutputStream.ToArray();
            OutputStream.Clear();
            return bytes;
        }

        public abstract void FireChannelActive();
        public virtual void WriteAndFlushAsync(IRequestMessage message)
        {
            this.WriteAndFlushAsync(message.ToByteBuffer().ToArray());
        }
        public abstract void WriteAndFlushAsync(byte[] message);

        protected void WriteAsync(byte[] message, bool flush)
        {
            OutputStream.WriteBytes(message);
            if (flush == true)
            {
                this.Channel.Flush();
            }
        }
    }
}
