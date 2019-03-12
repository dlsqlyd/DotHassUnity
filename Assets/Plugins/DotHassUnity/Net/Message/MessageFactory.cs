using HFramework.Net.Abstractions;
using System;
using System.Text;

namespace HFramework.Net.Message
{
    public static class MessageFactory
    {
        public static Func<RequestMessagePacket, ResponseMessagePacket> FakeSource;

        public static IResponseMessage Parse(byte[] bytes)
        {
            var packer = new ResponseMessagePacket();

            if (packer.Parse(new ByteBuffer(bytes)) == false)
            {
                return null;
            }
            return packer;
        }

        /// <summary>
        /// 设置clientid和sid
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="pipeline"></param>
        public static void ParseConnectionInfo(byte[] bytes, IChannelPipeline pipeline)
        {
            var str = Encoding.UTF8.GetString(bytes);
            var connectioninfo = str.Split('|');
            pipeline.ClientID = Convert.ToInt32(connectioninfo[0]);
            pipeline.SessionID = connectioninfo[1];
        }

        public static IResponseMessage Fake(IRequestMessage message)
        {
            return FakeSource(message as RequestMessagePacket);
        }

        internal static IRequestMessage Create(int clientid, string sessionid, int contractid, string sign, byte[] body)
        {
            return new RequestMessagePacket(clientid, sessionid, contractid, MessageIDCreater.GetNewID(), sign, body);
        }

        internal static IRequestMessage Create(int clientid, string sessionid, ContractType contract)
        {
            return new RequestMessagePacket(clientid, sessionid, (int)contract, MessageIDCreater.GetNewID(), "", null);
        }
    }
}
