using HFramework.Net.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HFramework.Net
{
    class FakeChannel : AbstractSocketChannel
    {
        protected override SocketChannelAsyncOperation WriteOperation => null;
        public FakeChannel(ChannelOptions options) : base(options)
        {
            this.Pipeline = new FakePipeline(this);
        }
        public override async Task<IChannelPipeline> ConnectAsync()
        {
            this.Connect0();
            return await Task.FromResult(this.Pipeline);
        }
        protected override void Connect0()
        {
            this.Pipeline.FireChannelActive();
        }
    }
}
