using DotHass.Unity.Net.Abstractions;
using DotHass.Unity.Net.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotHass.Unity.Net
{
    class FakePipeline : DefaultChannelPipeline
    {
        public FakePipeline(IChannel channel) : base(channel)
        {

        }
        public override void FireChannelActive()
        {
            if (OnHandShake != null)
            {
                this.OnHandShake(this);
            }
        }
        public override void WriteAndFlushAsync(IRequestMessage message)
        {
            if (OnRead != null)
            {
                this.OnRead(this, MessageFactory.Fake(message));
            }
        }
        public override void WriteAndFlushAsync(byte[] message)
        {

        }
    }
}
