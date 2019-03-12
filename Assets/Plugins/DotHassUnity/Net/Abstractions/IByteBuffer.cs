using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetty.Buffers
{
    public interface IByteBuffer
    {
        IByteBuffer SkipBytes(int length);
        void WriteByte(int val);
        void WriteByte(byte val);
        int ReadInt();
        void WriteInt(int messageID);
        byte ReadByte();
        void WriteBytes(byte[] bodyContent);
        void ReadBytes(byte[] bodyContent);


        int WriteString(string value, Encoding encoding);
        string ReadString(int length, Encoding encoding);


        byte[] ToArray();
    }
}