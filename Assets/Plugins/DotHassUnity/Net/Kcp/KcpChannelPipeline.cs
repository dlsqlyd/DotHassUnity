using HFramework.Net.Abstractions;
using HFramework.Net.Message;
using System;
using System.Threading;

namespace HFramework.Net.Kcp
{
    class KcpChannelPipeline : DefaultChannelPipeline
    {
        private KCP mKcp;
        private static readonly DateTime utc_time = new DateTime(1970, 1, 1);
        private static UInt32 iclock()
        {
            return (UInt32)(Convert.ToInt64(DateTime.UtcNow.Subtract(utc_time).TotalMilliseconds) & 0xffffffff);
        }
        private SwitchQueue<byte[]> mRecvQueue = new SwitchQueue<byte[]>(128);

        private bool mNeedUpdateFlag;
        private UInt32 mNextUpdateTime;
        private KcpOptions options;
        private Timer timer;

        private Timer updateKcpTimer;

        public KcpChannelPipeline(KcpSocketChannel channel, KcpOptions kcpOptions) : base(channel)
        {
            this.options = kcpOptions;

            this.updateKcpTimer = new System.Threading.Timer((e) =>
            {
                update(iclock());
            }, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(33));

            this.timer = new System.Threading.Timer((e) =>
            {
                this.HeatBeat();
            }, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
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

            if (this.ClientID == 0)
            {
                IResponseMessage message = null;
                try
                {
                    message = MessageFactory.Parse(bytes);
                }
                catch (Exception)
                {
                }

                if (message != null && message.ContractID == (int)ContractType.HandShake)
                {
                    if (mKcp != null)
                    {
                        this.mKcp.Dispose();
                    }
                    MessageFactory.ParseConnectionInfo(message.BodyContent, this);
                    this.init_kcp((uint)this.ClientID);
                }
            }
            else
            {
                mRecvQueue.Push(bytes);
            }
        }

        private void init_kcp(UInt32 conv)
        {
            mKcp = new KCP(conv, (byte[] kcppack, int size) =>
            {
                var bytes = new byte[size];
                Array.Copy(kcppack, bytes, size);
                this.WriteAsync(bytes, true);
            });
            // fast mode.
            mKcp.NoDelay(options.NoDelay, options.NoDelayInterval, options.NoDelayResend, options.NoDelayNC);
            mKcp.WndSize(options.SndWindowSize, options.RecWindowSize);
        }
        public override void WriteAndFlushAsync(byte[] message)
        {
            mKcp.Send(message, message.Length);
            mNeedUpdateFlag = true;
        }


        private void update(UInt32 current)
        {
            if (mKcp == null)
            {
                return;
            }
            process_recv_queue();

            if (mNeedUpdateFlag || current >= mNextUpdateTime)
            {
                mKcp.Update(current);
                mNextUpdateTime = mKcp.Check(current);
                mNeedUpdateFlag = false;
            }
        }

        private void process_recv_queue()
        {
            mRecvQueue.Switch();

            while (!mRecvQueue.Empty())
            {
                var buf = mRecvQueue.Pop();

                mKcp.Input(buf);
                mNeedUpdateFlag = true;

                for (var size = mKcp.PeekSize(); size > 0; size = mKcp.PeekSize())
                {
                    var buffer = new byte[size];
                    if (mKcp.Recv(buffer) > 0)
                    {
                        OnRead(this, MessageFactory.Parse(buffer));
                    }
                }
            }
        }
        /// <summary>
        /// 心跳是直接发送,不进行kcp封装
        /// </summary>
        private void HeatBeat()
        {
            if (this.ClientID == 0)
            {
                return;
            }
            var packet = MessageFactory.Create(this.ClientID, this.SessionID, ContractType.HeartBeat);
            var bytes = packet.ToByteBuffer().ToArray();
            WriteAsync(bytes, true);
        }


    }
}
