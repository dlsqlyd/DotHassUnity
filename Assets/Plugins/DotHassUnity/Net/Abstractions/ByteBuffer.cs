using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HFramework.Net.Abstractions
{
    /**
     * 类似netty4的bytebuf
     * @author StanWind
     * @data 2016年08月23日10:13:29
     */
    public class ByteBuffer : IByteBuffer
    {
        //字节缓存区
        private byte[] buf;
        //读取索引
        private int readerIndex = 0;
        //写入索引
        private int writerIndex = 0;
        //读取索引标记
        private int markedReaderIndex = 0;
        //写入索引标记
        private int markedWriterIndex = 0;
        //缓存区字节数组的长度
        private int maxCapacity;
        public int Capacity { get { return this.maxCapacity; } }
        /**
         * 构造方法
         */
        public ByteBuffer(int capacity)
        {
            buf = new byte[capacity];
            this.maxCapacity = capacity;
        }

        /**
         * 构造方法
         */
        public ByteBuffer(byte[] bytes)
        {
            buf = bytes;
            this.maxCapacity = bytes.Length;
        }



        /**
         * 构建一个capacity长度的字节缓存区ByteBuffer对象
         */
        public static ByteBuffer Allocate(int capacity)
        {
            return new ByteBuffer(capacity);
        }

        /**
         * 构建一个以bytes为字节缓存区的ByteBuffer对象，一般不推荐使用
         */
        public static ByteBuffer Allocate(byte[] bytes)
        {
            return new ByteBuffer(bytes);
        }

        /**
         * 根据length长度，确定大于此leng的最近的2次方数，如length=7，则返回值为8
         */
        private int FixLength(int length)
        {
            int n = 2;
            int b = 2;
            while (b < length)
            {
                b = 2 << n;
                n++;
            }
            return b;
        }

        /**
         * 翻转字节数组，如果本地字节序列为低字节序列，则进行翻转以转换为高字节序列
         */
        private byte[] flip(byte[] bytes)
        {
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }
            return bytes;
        }

        /**
         * 确定内部字节缓存数组的大小
         */
        private int FixSizeAndReset(int currLen, int futureLen)
        {
            if (futureLen > currLen)
            {
                //以原大小的2次方数的两倍确定内部字节缓存区大小
                int size = FixLength(currLen) * 2;
                if (futureLen > size)
                {
                    //以将来的大小的2次方的两倍确定内部字节缓存区大小
                    size = FixLength(futureLen) * 2;
                }
                byte[] newbuf = new byte[size];
                Array.Copy(buf, 0, newbuf, 0, currLen);
                buf = newbuf;
                maxCapacity = newbuf.Length;
            }
            return futureLen;
        }

        public virtual IByteBuffer SkipBytes(int length)
        {
            this.readerIndex += length;
            return this;
        }

        /**
         * 将bytes字节数组从startIndex开始的length字节写入到此缓存区
         */
        public void WriteBytes(byte[] bytes, int startIndex, int length)
        {
            lock (this)
            {
                int offset = length - startIndex;
                if (offset <= 0) return;
                int total = offset + writerIndex;
                int len = buf.Length;
                FixSizeAndReset(len, total);
                for (int i = writerIndex, j = startIndex; i < total; i++, j++)
                {
                    buf[i] = bytes[j];
                }
                writerIndex = total;
            }
        }

        /**
         * 将字节数组中从0到length的元素写入缓存区
         */
        public void WriteBytes(byte[] bytes, int length)
        {
            WriteBytes(bytes, 0, length);
        }

        /**
         * 将字节数组全部写入缓存区
         */
        public void WriteBytes(byte[] bytes)
        {
            WriteBytes(bytes, bytes.Length);
        }

        /**
         * 将一个ByteBuffer的有效字节区写入此缓存区中
         */
        public void Write(ByteBuffer buffer)
        {
            if (buffer == null) return;
            if (buffer.ReadableBytes() <= 0) return;
            WriteBytes(buffer.ToArray());
        }

        /**
         * 写入一个int16数据
         */
        public void WriteShort(short value)
        {
            WriteBytes(flip(BitConverter.GetBytes(value)));
        }

        /**
         * 写入一个uint16数据
         */
        public void WriteUshort(ushort value)
        {
            WriteBytes(flip(BitConverter.GetBytes(value)));
        }

        /**
         * 写入一个int32数据
         */
        public void WriteInt(int value)
        {
            WriteBytes(flip(BitConverter.GetBytes(value)));
        }

        /**
         * 写入一个uint32数据
         */
        public void WriteUint(uint value)
        {
            WriteBytes(flip(BitConverter.GetBytes(value)));
        }

        /**
         * 写入一个int64数据
         */
        public void WriteLong(long value)
        {
            WriteBytes(flip(BitConverter.GetBytes(value)));
        }

        /**
         * 写入一个uint64数据
         */
        public void WriteUlong(ulong value)
        {
            WriteBytes(flip(BitConverter.GetBytes(value)));
        }

        /**
         * 写入一个float数据
         */
        public void WriteFloat(float value)
        {
            WriteBytes(flip(BitConverter.GetBytes(value)));
        }
    
        public int WriteString(string value, Encoding encoding)
        {
            byte[] b = encoding.GetBytes(value);
            WriteBytes(b);
            return b.Length;
        }
      
        /**
         * 写入一个byte数据
         */
        public void WriteByte(byte value)
        {
            lock (this)
            {
                int afterLen = writerIndex + 1;
                int len = buf.Length;
                FixSizeAndReset(len, afterLen);
                buf[writerIndex] = value;
                writerIndex = afterLen;
            }
        }

        public void WriteByte(int value)
        {
            WriteByte((byte)value);
        }

        /**
         * 写入一个double类型数据
         */
        public void WriteDouble(double value)
        {
            WriteBytes(flip(BitConverter.GetBytes(value)));
        }

        /**
         * 读取一个字节
         */
        public byte ReadByte()
        {
            byte b = buf[readerIndex];
            readerIndex++;
            return b;
        }


        /**
         * 从读取索引位置开始读取len长度的字节数组
         */
        private byte[] Read(int len)
        {
            byte[] bytes = new byte[len];
            Array.Copy(buf, readerIndex, bytes, 0, len);
            readerIndex += len;
            return flip(bytes);
        }


        /**
         * 读取一个string数据 
         */
        public string ReadString(int length, Encoding encoding)
        {
            byte[] b = new byte[length];
            ReadBytes(b, 0, length);
            return Encoding.UTF8.GetString(b);
        }
               
        /**
         * 读取一个uint16数据
         */
        public ushort ReadUshort()
        {
            return BitConverter.ToUInt16(Read(2), 0);
        }

        /**
         * 读取一个int16数据
         */
        public short ReadShort()
        {
            return BitConverter.ToInt16(Read(2), 0);
        }

        /**
         * 读取一个uint32数据
         */
        public uint ReadUint()
        {
            return BitConverter.ToUInt32(Read(4), 0);
        }

        /**
         * 读取一个int32数据
         */
        public int ReadInt()
        {
            return BitConverter.ToInt32(Read(4), 0);
        }

        /**
         * 读取一个uint64数据
         */
        public ulong ReadUlong()
        {
            return BitConverter.ToUInt64(Read(8), 0);
        }

        /**
         * 读取一个long数据
         */
        public long ReadLong()
        {
            return BitConverter.ToInt64(Read(8), 0);
        }

        /**
         * 读取一个float数据
         */
        public float ReadFloat()
        {
            return BitConverter.ToSingle(Read(4), 0);
        }

        /**
         * 读取一个double数据
         */
        public double ReadDouble()
        {
            return BitConverter.ToDouble(Read(8), 0);
        }

        /**
         * 从读取索引位置开始读取len长度的字节到disbytes目标字节数组中
         * @params disstart 目标字节数组的写入索引
         */
        public void ReadBytes(byte[] disbytes, int disstart, int len)
        {
            int size = disstart + len;
            for (int i = disstart; i < size; i++)
            {
                disbytes[i] = this.ReadByte();
            }
        }

        public void ReadBytes(byte[] dst)
        {
            this.ReadBytes(dst, 0, dst.Length);
        }

        /**
         * 清除已读字节并重建缓存区
         */
        public void DiscardReadBytes()
        {
            if (readerIndex <= 0) return;
            int len = buf.Length - readerIndex;
            byte[] newbuf = new byte[len];
            Array.Copy(buf, readerIndex, newbuf, 0, len);
            buf = newbuf;
            writerIndex -= readerIndex;
            markedReaderIndex -= readerIndex;
            if (markedReaderIndex < 0)
            {
                markedReaderIndex = readerIndex;
            }
            markedWriterIndex -= readerIndex;
            if (markedWriterIndex < 0 || markedWriterIndex < readerIndex || markedWriterIndex < markedReaderIndex)
            {
                markedWriterIndex = writerIndex;
            }
            readerIndex = 0;
        }

        /**
         * 清空此对象
         */
        public void Clear()
        {
            buf = new byte[buf.Length];
            readerIndex = 0;
            writerIndex = 0;
            markedReaderIndex = 0;
            markedWriterIndex = 0;
        }

        /**
         * 设置开始读取的索引
         */
        public void SetReaderIndex(int index)
        {
            if (index < 0 || index > this.writerIndex)
            {
                throw new IndexOutOfRangeException($"ReaderIndex: {index} (expected: 0 <= readerIndex <= writerIndex({this.writerIndex})");
            }
            readerIndex = index;
        }
        public virtual IByteBuffer SetWriterIndex(int index)
        {
            if (index < this.readerIndex || index > this.Capacity)
            {
                throw new IndexOutOfRangeException($"WriterIndex: {index} (expected: 0 <= readerIndex({this.readerIndex}) <= writerIndex <= capacity ({this.Capacity})");
            }

            this.writerIndex = index;
            return this;
        }
        /**
         * 标记读取的索引位置
         */
        public void MarkReaderIndex()
        {
            markedReaderIndex = readerIndex;
        }

        /**
         * 标记写入的索引位置
         */
        public void MarkWriterIndex()
        {
            markedWriterIndex = writerIndex;
        }

        /**
         * 将读取的索引位置重置为标记的读取索引位置
         */
        public void ResetReaderIndex()
        {
            readerIndex = markedReaderIndex;
        }

        /**
         * 将写入的索引位置重置为标记的写入索引位置
         */
        public void ResetWriterIndex()
        {
            writerIndex = markedWriterIndex;
        }

        /**
         * 可读的有效字节数
         */
        public int ReadableBytes()
        {
            return writerIndex - readerIndex;
        }

        /**
         * 获取可读的字节数组
         */
        public byte[] ToArray()
        {
            byte[] bytes = new byte[writerIndex];
            Array.Copy(buf, 0, bytes, 0, bytes.Length);
            return bytes;
        }
    }
}
