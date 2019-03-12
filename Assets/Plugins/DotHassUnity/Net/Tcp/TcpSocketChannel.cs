using HFramework.Net.Abstractions;
using HFramework.Net.Tcp;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace HFramework.Net
{
    public class TcpSocketChannel : AbstractSocketChannel
    {
        private SocketChannelAsyncOperation writeOperation;
        protected override SocketChannelAsyncOperation WriteOperation => this.writeOperation ?? (this.writeOperation = new SocketChannelAsyncOperation(this, true));

        public override Socket Socket
        {
            get
            {
                if (_socket == null)
                {
                    _socket = new Socket(this.RemoteAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                }

                return _socket;
            }
        }


        public TcpSocketChannel(ChannelOptions options) : base(options)
        {
            this.Pipeline = new TcpChannelPipeline(this);
        }



        protected override void Connect0()
        {
            SocketChannelAsyncOperation eventPayload = new SocketChannelAsyncOperation(this, true);
            eventPayload.RemoteEndPoint = RemoteAddress;
            bool connected = this.Socket.ConnectAsync(eventPayload);
            if (connected == false)
            {
                //connected不代表连接失败。。代表不会触发complete事件
                this.FinishConnect(eventPayload);
            }
        }

        public override IChannel Flush()
        {
            if (this.inFlush)
            {
                return this;
            }
            this.inFlush = true;
            return base.Flush();
        }
    }
}
