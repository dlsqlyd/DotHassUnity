using HFramework.Net.Abstractions;
using HFramework.Net.Message;
using System;
using System.IO;
using System.Threading;

namespace HFramework.Net.Tcp
{
    class TcpChannelPipeline : DefaultChannelPipeline
    {
        private MemoryStream InputStream;

        private BinaryReader reader;
        private Timer timer;

        public TcpChannelPipeline(IChannel channel) : base(channel)
        {
            this.InputStream = new MemoryStream();
            reader = new BinaryReader(InputStream);
            this.timer = new System.Threading.Timer((e) =>
            {
                this.HeatBeat();
            }, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
        }

        /// <summary>
        /// 有sessionid后在active
        /// </summary>
        public override void FireChannelActive()
        {

        }


        public override void FireChannelRead(byte[] buffer, int offset, int received)
        {
            InputStream.Seek(0, SeekOrigin.End);
            InputStream.Write(buffer, offset, received);
            InputStream.Seek(0, SeekOrigin.Begin);

            //如果消息流有消息头的长度
            while (RemainingBytes(InputStream) > 2)
            {
                //消息长度
                short messageLen = reader.ReadInt16();
                //如果消息流中有足够的消息body数据
                if (RemainingBytes(InputStream) >= messageLen)
                {
                    var bytes = reader.ReadBytes(messageLen);
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
                        if (OnHandShake != null)
                        {
                            this.OnHandShake(this);
                        }
                    }
                    else
                    {
                        this.OnRead(this, message);
                    }
                }
                else
                {  //如果没有足够的数据，则将指针移到消息头长度前
                    InputStream.Position = InputStream.Position - 2;
                    break;
                }
            }
            //剩下的字节
            byte[] leftover = reader.ReadBytes((int)RemainingBytes(InputStream));
            InputStream.SetLength(0);
            InputStream.Write(leftover, 0, leftover.Length);
        }

        private long RemainingBytes(MemoryStream memStream)
        {
            return memStream.Length - memStream.Position;
        }

        public override void WriteAndFlushAsync(byte[] message)
        {
            this.OutputStream.WriteShort((short)message.Length);
            this.WriteAsync(message, true);
        }

        /// <summary>
        /// 心跳需要使用WriteAndFlushAsync加个包长度
        /// </summary>
        private void HeatBeat()
        {
            if (this.ClientID == 0)
            {
                return;
            }
            var packet = MessageFactory.Create(this.ClientID, this.SessionID, ContractType.HeartBeat);
            var bytes = packet.ToByteBuffer().ToArray();
            WriteAndFlushAsync(bytes);
        }
    }
}
