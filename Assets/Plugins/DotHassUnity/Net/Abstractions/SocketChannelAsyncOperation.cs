// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace HFramework.Net.Abstractions
{
    using HFramework.Net.Utility;
    using System.Diagnostics.Contracts;
    using System.Net.Sockets;

    public class SocketChannelAsyncOperation : SocketAsyncEventArgs
    {
        public static int MaxAllocBufferSize = 8192;


        public SocketChannelAsyncOperation(AbstractSocketChannel channel, bool setEmptyBuffer)
        {
            Contract.Requires(channel != null);

            this.Channel = channel;
            this.Completed += AbstractSocketChannel.IoCompletedCallback;
            if (setEmptyBuffer == false)
            {
                this.SetBuffer(new byte[MaxAllocBufferSize], 0, MaxAllocBufferSize);
            }
            else
            {
                this.SetBuffer(ArrayExtensions.ZeroBytes, 0, 0);
            }
        }

        public void Validate()
        {
            SocketError socketError = this.SocketError;
            if (socketError != SocketError.Success)
            {
                throw new SocketException((int)socketError);
            }
        }

        public AbstractSocketChannel Channel { get; private set; }
    }
}