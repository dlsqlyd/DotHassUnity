using DotHass.Unity.Net.Abstractions;
using DotHass.Unity.Net.Message;
using System;

namespace DotHass.Unity.Net.Udp
{
    class UdpChannelPipeline : DefaultChannelPipeline
    {

        public UdpChannelPipeline(IChannel channel) : base(channel)
        {

        }


        public override void FireChannelActive()
        {
            if (OnHandShake != null)
            {
                this.OnHandShake(this);
            }
        }

        /// <summary>
        /// 和tcp不同。。。会先触发一次read。。再active
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="received"></param>
        public override void FireChannelRead(byte[] buffer, int offset, int received)
        {
            byte[] bytes = new byte[received];
            Array.Copy(buffer, offset, bytes, 0, bytes.Length);

            IResponseMessage message = null;
            try
            {
                message = MessageFactory.Parse(bytes);
            }
            catch (Exception)
            {
            }

            if (message == null)
            {
                return;
            }

            if (this.ClientID == 0 && message.ContractID == (int)ContractType.HandShake)
            {
                MessageFactory.ParseConnectionInfo(message.BodyContent, this);
            }
            else
            {
                this.OnRead(this, message);
            }
        }

        public override void WriteAndFlushAsync(byte[] message)
        {
            this.WriteAsync(message, true);
        }
    }
}
