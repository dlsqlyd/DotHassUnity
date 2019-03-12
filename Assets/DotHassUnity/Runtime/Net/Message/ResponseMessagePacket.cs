using DotHass.Unity.Net.Abstractions;
using DotHass.Unity.Net.Utility;
using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotHass.Unity.Net.Message
{

    public class ResponseMessagePacket : IResponseMessage
    {
        public MessageHeader Header { get; set; }
        public byte[] BodyContent { get; set; }
        public int ContractID { get { return Header.ContractID; } }
        public int StateCode { get { return Header.StateCode; } }
        public int MessageID { get { return Header.MessageID; } }
        public ResponseMessagePacket()
        {
        }
        public bool Parse(IByteBuffer buffer)
        {
            try
            {
                Header = MessageHeader.Parse(buffer);
                //消息体由协议序列化后的内容构成
                if (Header.MessageBodyLen > 0)
                {
                    this.BodyContent = new byte[Header.MessageBodyLen];
                    buffer.ReadBytes(this.BodyContent);
                }
                else//保证protobuffer对象默认状态下可反序列化(protobuffer序列化对象默认值时，字节数组为0)
                {
                    this.BodyContent = new byte[0];
                }
                return true;
            }
            catch (Exception)
            {
            }

            return false;
        }


        public class MessageHeader
        {
            public Int32 StateCode { get; set; }
            public Int32 ContractID { get; set; }
            public Int32 MessageID { get; set; }
            public Int32 MessageBodyLen { get; set; }
            public MessageHeader() { }

            public static MessageHeader Parse(IByteBuffer buffer)
            {
                var header = new MessageHeader();
                header.StateCode = buffer.ReadInt();
                header.ContractID = buffer.ReadInt();
                header.MessageID = buffer.ReadInt();
                header.MessageBodyLen = buffer.ReadInt();
                return header;
            }
        }
    }

}
