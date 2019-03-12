using DotNetty.Buffers;

namespace HFramework.Net.Abstractions
{
    public interface IRequestMessage
    {
        IByteBuffer ToByteBuffer();
    }
}
