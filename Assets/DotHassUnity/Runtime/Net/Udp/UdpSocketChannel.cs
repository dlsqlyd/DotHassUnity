using DotHass.Unity.Net.Abstractions;
using DotHass.Unity.Net.Message;
using System;
using System.Net;
using System.Net.Sockets;

namespace DotHass.Unity.Net.Udp
{

    /// <summary>
    /// 测试：
    /// 1.服务器未开启就连接的时候。read出现错误。后者虽然连接失败但不会出现错误
    /// 2.主机关闭的时候。read出现错误
    /// 3.超时后，read出现错误
    /// </summary>
    class UdpSocketChannel : AbstractSocketChannel
    {
        public override Socket Socket
        {
            get
            {
                if (_socket == null)
                {
                    _socket = new Socket(this.RemoteAddress.AddressFamily, SocketType.Dgram, ProtocolType.Udp);
                }

                return _socket;
            }
        }

        protected override SocketChannelAsyncOperation WriteOperation
        {
            get
            {
                return new SocketChannelAsyncOperation(this, true);
            }
        }


        public UdpSocketChannel(ChannelOptions options) : base(options)
        {
            this.Pipeline = new UdpChannelPipeline(this);
        }


        protected override void Connect0()
        {
            var readOperation = new SocketChannelAsyncOperation(this, false);
            readOperation.RemoteEndPoint = new IPEndPoint(Socket.AddressFamily == AddressFamily.InterNetwork ? IPAddress.Any : IPAddress.IPv6Any, IPEndPoint.MinPort);
            Boolean willRaiseEvent = this.Socket.ReceiveFromAsync(readOperation);
            if (!willRaiseEvent)
            {
                this.FinishRead(readOperation);
            }
            var ack = MessageFactory.Create(this.Pipeline.ClientID, this.Pipeline.SessionID, ContractType.HandShake);
            var data = ack.ToByteBuffer().ToArray();
            var e = WriteOperation;
            e.RemoteEndPoint = this.RemoteAddress;
            e.SetBuffer(data, 0, data.Length);
            Boolean willRaiseSendEvent = this.Socket.SendToAsync(e);
            if (!willRaiseSendEvent)
            {
                this.FinishWrite(e);
            }
        }

        public override void FinishRead(SocketChannelAsyncOperation operation)
        {
            base.FinishRead(operation);
            if (this.state == StateFlags.Close)
            {
                return;
            }
            if (operation.RemoteEndPoint != null && operation.RemoteEndPoint.ToString() != this.RemoteAddress.ToString())
            {
                this.Socket.Connect(operation.RemoteEndPoint);
                this.FinishConnect(null);
            }
        }

    }
}
