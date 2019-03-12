using HFramework.Net.Utility;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine;
namespace HFramework.Net.Abstractions
{
    public enum StateFlags
    {
        Open,
        Active,
        Close
    }

    public abstract class AbstractSocketChannel : IChannel
    {

        public int ConnectionId { get; set; }
        public IChannelPipeline Pipeline { get; set; }
        public ChannelOptions Options { get; }
        public IPEndPoint RemoteAddress { get; }

        internal static readonly EventHandler<SocketAsyncEventArgs> IoCompletedCallback = OnIoCompleted;

        protected Socket _socket;
        public virtual Socket Socket { get { return _socket; } }

        public volatile StateFlags state;

        protected bool inFlush;


        protected abstract SocketChannelAsyncOperation WriteOperation { get; }
        protected SocketChannelAsyncOperation readOperation;
        protected virtual SocketChannelAsyncOperation ReadOperation => this.readOperation ?? (this.readOperation = new SocketChannelAsyncOperation(this, false));



        public TaskCompletionSource<IChannelPipeline> connectTask;


        public AbstractSocketChannel(ChannelOptions options)
        {
            this.ConnectionId = CorrelationIdGenerator.GetNextId();
            this.Options = options;
            this.RemoteAddress = new IPEndPoint(IPAddress.Parse(Options.IP), Options.Port); 
        }


        public virtual async Task<IChannelPipeline> ConnectAsync()
        {
            if (_socket != null)
            {
                throw new InvalidOperationException("TransportAlreadyBound");
            }
            try
            {
                connectTask = new TaskCompletionSource<IChannelPipeline>();
                //当握手成功的时候才代表完成连接
                this.Pipeline.OnHandShake -= OnHandShake;
                this.Pipeline.OnHandShake += OnHandShake;

                this.Socket.Blocking = true;
                this.state = StateFlags.Open;
                //Kestrel expects IPv6Any to bind to both IPv6 and IPv4
                if (this.RemoteAddress.Address == IPAddress.IPv6Any)
                {
                    Socket.DualMode = true;
                }
                this.Connect0();
            }
            catch (Exception ex)
            {
                Debug.LogError("connect error in Connect" + ex);
                this.ConnectError();
            }

            return await connectTask.Task;
        }

        private void OnHandShake(IChannelPipeline pipe)
        {
            connectTask.SetResult(pipe);
        }


        public  void ReConnectAsync()
        {
            if (this.state != StateFlags.Close)
            {
                return ;
            }
   
            Debug.Log("断线重联中");
            this.Connect0();
        }


        protected abstract void Connect0();

        private static void OnIoCompleted(object sender, SocketAsyncEventArgs args)
        {
            var operation = (SocketChannelAsyncOperation)args;
            AbstractSocketChannel channel = operation.Channel;

            switch (args.LastOperation)
            {
                case SocketAsyncOperation.Connect:
                    channel.FinishConnect(operation);
                    break;
                case SocketAsyncOperation.Receive:
                case SocketAsyncOperation.ReceiveFrom:
                    channel.FinishRead(operation);
                    break;
                case SocketAsyncOperation.Send:
                case SocketAsyncOperation.SendTo:
                    channel.FinishWrite(operation);
                    break;
                default:
                    // todo: think of a better way to comm exception
                    throw new ArgumentException("The last operation completed on the socket was not expected");
            }
        }



        public virtual void FinishConnect(SocketChannelAsyncOperation operation)
        {
            if (this.state == StateFlags.Active)
            {
                return;
            }
            this.state = StateFlags.Active;
            try
            {
                if (operation != null)
                {
                    operation.Validate();
                }
                this.Receive();
                this.Pipeline.FireChannelActive();
            }
            catch (Exception ex)
            {
                Debug.LogError("connect error in finishconnect" + ex);
                this.ConnectError();
            }
        }

        public virtual void FinishRead(SocketChannelAsyncOperation operation)
        {
            bool close = false;
            try
            {
                int received = operation.BytesTransferred;
                SocketError errorCode = operation.SocketError;

                switch (errorCode)
                {
                    case SocketError.Success:
                        if (received == 0)
                        {
                            close = true; // indicate that socket was closed
                        }
                        else
                        {
                            try
                            {
                                Pipeline.FireChannelRead(operation.Buffer, operation.Offset, received);
                            }
                            catch (Exception ex)
                            {

                                Debug.LogError("Pipeline hander error" + ex);
                            }

                        }
                        break;
                    default:
                        throw new SocketException((int)errorCode);
                }
                if (this.state == StateFlags.Active)
                {
                    this.Receive();
                }
                if (close)
                {
                    Debug.LogError("connect error in FinishRead");
                    this.ConnectError();
                }
            }
            catch (Exception ex)
            {   //当服务器关闭的时候会触发这个
                Debug.LogError("read data error in FinishRead" + ex);
                this.ConnectError();
            }
        }

        protected virtual void Receive()
        {
            Boolean willRaiseEvent = this.Socket.ReceiveAsync(ReadOperation);
            if (!willRaiseEvent)
            {
                this.FinishRead(ReadOperation);
            }
        }

        public virtual void FinishWrite(SocketChannelAsyncOperation operation)
        {
            try
            {
                operation.Validate();
                this.inFlush = false;
            }
            catch (Exception ex)
            {
                Debug.LogError("write data error in FinishWrite" + ex);
            }
            this.Flush();//如果还有数据的话会再次写入
        }
        /// <summary>
        /// 从管道里读取
        /// </summary>
        /// <returns></returns>
        public virtual IChannel Flush()
        {
            byte[] outboundBuffer = this.Pipeline.GetOutputBuffer();
            if (outboundBuffer == null || outboundBuffer.Length == 0)
            {
                this.inFlush = false;
                return this;
            }
            this.Flush0(outboundBuffer);
            return this;
        }

        /// <summary>
        /// 可在channel中直接调用
        /// </summary>
        /// <param name="data"></param>
        protected void Flush0(byte[] data)
        {
            try
            {
                var e = WriteOperation;
                e.SetBuffer(data, 0, data.Length);
                Boolean willRaiseEvent = this.Socket.SendAsync(e);
                if (!willRaiseEvent)
                {
                    this.FinishWrite(e);
                }
            }
            catch (Exception t)
            {
                Debug.LogError("write erro in flush" + t);
            }
        }

        public Task CloseAsync()
        {
            this.DoClose();
            return Task.CompletedTask;
        }

        public void ConnectError()
        {
            if (this.state == StateFlags.Close)
            {
                return;
            }
            this.DoClose();
            this.Pipeline.FireConnectError();
        }

        protected virtual void DoClose()
        {
            try
            {
                if (this.state != StateFlags.Close)
                {
                    var socket = this._socket;
                    this._socket = null;
                    this.inFlush = false;
                    this.state = StateFlags.Close;

                    //在未连接到服务器的时候可能会异常，又没有捕捉错误可能会不继续执行doclose后的内容
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Dispose();
                }
            }
            catch (Exception t)
            {
                Debug.LogError("close error : " + t);
            }
        }

        public bool IsActive()
        {
            return state == StateFlags.Active;
        }
    }

}



