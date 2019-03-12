using DotHass.Unity.Net.Abstractions;
using DotHass.Unity.Net.Utility;
using DotNetty.Buffers;
using System;
using System.Text;

namespace DotHass.Unity.Net.Message
{
    /// <summary>
    /// 消息包，消息头+协议id+协议体
    /// </summary>
    public class RequestMessagePacket : IRequestMessage
    {
        public MessageHeader Header { get; set; }
        public byte[] BodyContent { get; set; }
        public int MessageID { get { return Header.MessageID; } }
        public int Length { get { return 16 + 2 + Encoding.UTF8.GetByteCount(Header.SessionID) + Encoding.UTF8.GetByteCount(Header.Sign) + Header.MessageBodyLen; } }


        public RequestMessagePacket(int clientid, string sessionID, int contractID, int messageID, string sign, byte[] body)
        {
            this.BodyContent = body ?? new byte[0];
            this.Header = new MessageHeader()
            {
                ClientID = clientid,
                MessageID = messageID,
                ContractID = contractID,
                SessionID = sessionID,
                Sign = sign,
                MessageBodyLen = body == null ? 0 : body.Length,
            };
        }
        public IByteBuffer ToByteBuffer()
        {
            var buffer = ByteBuffer.Allocate(this.Length);
            this.Header.Serialize(buffer);
            if (this.BodyContent != null)
            {
                buffer.WriteBytes(this.BodyContent);
            }
            return buffer;
        }

        public class MessageHeader
        {
            public Int32 ClientID { get; set; }
            public Int32 MessageID { get; set; }
            public Int32 ContractID { get; set; }
            public string SessionID { get; set; }
            public string Sign { get; set; }
            public Int32 MessageBodyLen { get; set; }

            public MessageHeader() { }

            public void Serialize(IByteBuffer buffer)
            {
                buffer.WriteInt(this.ClientID);
                buffer.WriteInt(this.MessageID);
                buffer.WriteInt(this.ContractID);
                buffer.WriteShortString(this.SessionID, Encoding.UTF8);
                buffer.WriteShortString(this.Sign, Encoding.UTF8);
                buffer.WriteInt(this.MessageBodyLen);
            }
        }
    }



}
