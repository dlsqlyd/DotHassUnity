using DotHass.Unity.Net.Abstractions;
using DotHass.Unity.Net.Message;
using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DotHass.Unity.Net.Utility
{

    public static class ByteBufferExtension
    {
        public static string ReadShortString(this IByteBuffer buffer, Encoding encoding)
        {
            int length = buffer.ReadByte();
            return length <= 0 ? string.Empty : buffer.ReadString(length, encoding);
        }

        /// <summary>
        /// 字符存长度不能大于256...总长度等于1个字节+字符窜的长度
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="str"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static int WriteShortString(this IByteBuffer buffer, string str, Encoding encoding)
        {
            byte[] bytes = encoding.GetBytes(str);
            buffer.WriteByte(bytes.Length);
            buffer.WriteBytes(bytes);
            return bytes.Length;
        }

        public static string ReadLongString(this IByteBuffer buffer, Encoding encoding)
        {
            int length = buffer.ReadInt();
            return length <= 0 ? string.Empty : buffer.ReadString(length, encoding);
        }

        public static int WriteLongString(this IByteBuffer buffer, string str, Encoding encoding)
        {
            byte[] bytes = encoding.GetBytes(str);
            buffer.WriteInt(bytes.Length);
            buffer.WriteBytes(bytes);
            return bytes.Length;
        }
    }




}
