using DotNetty.Buffers;

namespace DotHass.Unity.Net.Abstractions
{
    public interface IRequestMessage
    {
        IByteBuffer ToByteBuffer();
    }
}
