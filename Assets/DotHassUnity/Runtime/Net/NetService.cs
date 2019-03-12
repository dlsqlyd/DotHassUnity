using DotHass.Unity.Net.Abstractions;
using DotHass.Unity.Net.Kcp;
using DotHass.Unity.Net.Message;
using Elskom.Generic.Libs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DotHass.Unity.Net
{



    public class ErrorInfo
    {
        public int StateCode;

        public int MsgId;
    }


    /// <summary>
    /// 1.可以有多个transport
    /// 2.启动不同的transport
    /// 3.可以关闭不同的transport
    /// 4.分发事件
    /// </summary>
    public class NetService
    {
        public Dictionary<string, IChannel> Channels = new Dictionary<string, IChannel>();
        public Dictionary<int, GameAction> SendActions = new Dictionary<int, GameAction>();
        private Dictionary<string, BlowFish> crypts = new Dictionary<string, BlowFish>();
        private readonly Loom loom;

        public Action<IChannelPipeline, RequestMessagePacket, bool> SendCallBack { get; set; }
        public Action<IChannelPipeline, ResponseMessagePacket> ReadCallBack { get; set; }
        public Action<IChannelPipeline> ErrorCallBack { get; set; }
        public Action<IChannelPipeline> HandShakeCallBack { get; set; }


        public NetService()
        {
            //在主线程初始化loom
            this.loom = Loom.Current;
        }

        public async Task ConnectFake(ChannelOptions options)
        {
            if (Channels.TryGetValue(options.Name, out IChannel channel) == false)
            {
                channel = new FakeChannel(options);
                Channels.Add(options.Name, channel);
                channel.Pipeline.OnHandShake += OnHandShake;
                channel.Pipeline.OnRead += OnRead;
                channel.Pipeline.OnError += OnError;
            }

            await channel.ConnectAsync();
        }
        public async Task ConnectTcp(ChannelOptions options)
        {
            if (Channels.TryGetValue(options.Name, out IChannel channel) == false)
            {
                channel = new TcpSocketChannel(options);
                Channels.Add(options.Name, channel);
                channel.Pipeline.OnHandShake += OnHandShake;
                channel.Pipeline.OnRead += OnRead;
                channel.Pipeline.OnError += OnError;
            }

            await channel.ConnectAsync();
        }

        public IChannel GetChannel(string name)
        {
            return Channels[name];
        }

        public async Task ConnectKcp(ChannelOptions options, KcpOptions kcpOptions = null)
        {
            if (Channels.TryGetValue(options.Name, out IChannel channel) == false)
            {
                kcpOptions = kcpOptions ?? new KcpOptions();
                channel = new KcpSocketChannel(options, kcpOptions);
                Channels.Add(options.Name, channel);
                channel.Pipeline.OnHandShake += OnHandShake;
                channel.Pipeline.OnRead += OnRead;
                channel.Pipeline.OnError += OnError;
            }

            await channel.ConnectAsync();
        }


        private void OnError(IChannelPipeline obj)
        {
            if (this.ErrorCallBack == null)
            {
                return;
            }
            //当前不是主线程，，得添加到loom中
            Loom.QueueOnMainThread(() =>
            {
                ErrorCallBack.Invoke(obj);
            });
        }

        private void OnHandShake(IChannelPipeline pipe)
        {
            Debug.Log(string.Format("channel {0} 握手成功", pipe.Channel.Options.Name));

            if (this.HandShakeCallBack == null)
            {
                return;
            }
            //当前不是主线程，，得添加到loom中
            Loom.QueueOnMainThread(() =>
            {
                HandShakeCallBack.Invoke(pipe);
            });
        }


        //先检查消息id的回调是否存在.
        //如果不存在则调用action
        private void OnRead(IChannelPipeline pipe, IResponseMessage message)
        {
            var channelName = pipe.Channel.Options.Name;
            var msg = message as ResponseMessagePacket;

            if (this.ReadCallBack != null)
            {
                //当前不是主线程，，得添加到loom中
                Loom.QueueOnMainThread(() =>
                {
                    ReadCallBack.Invoke(pipe, msg);
                });
            }

            GameAction action;
            if (this.SendActions.ContainsKey(msg.MessageID))
            {
                action = this.SendActions[msg.MessageID];
                this.SendActions.Remove(msg.MessageID);
            }
            else
            {
                action = ActionFactory.CreateAction.Invoke(channelName, msg.ContractID);
            }
            if (action != null)
            {
                if (msg.StateCode == 200)
                {
                    action.Handler(msg.BodyContent);
                    return;
                }
                action.HandlerError(new ErrorInfo()
                {
                    StateCode = msg.StateCode,
                    MsgId = msg.MessageID
                });
            }
        }

        //需要等待和返回值的才使用这个
        public Task<object> Send(string channelName, short ContractID, Dictionary<string, string> sendParams, Action<ErrorInfo> errorCallback = null, bool loadMask = true)
        {
            var taskCompletionSource = new TaskCompletionSource<object>();
            this.Send(channelName, ContractID, sendParams, (object v) =>
            {
                //错误消息
                //TODO：如果是错误，taskCompletionSource是否需要取消
                if (v is ErrorInfo e)
                {
                    if (errorCallback != null)
                    {
                        //当前不是主线程，，得添加到loom中
                        Loom.QueueOnMainThread(() =>
                        {
                            errorCallback.Invoke(e);
                        });
                    }
                    return;
                }
                //当setresult后。。await会自动返回主线程
                taskCompletionSource.SetResult(v);
            }, loadMask);
            return taskCompletionSource.Task;
        }

        public void Send(string channelName, int ContractID, Dictionary<string, string> sendParams, Action<object> callback = null, bool loadMask = true)
        {
            var channel = this.Channels[channelName];
            var body = CreateBody(sendParams);

            var signStr = channel.Pipeline.ClientID + channel.Pipeline.SessionID + ContractID + body.Length.ToString();

            var sign = GetCrypt(channel.Options.key).EncryptECB(signStr);

            var message = MessageFactory.Create(channel.Pipeline.ClientID, channel.Pipeline.SessionID, ContractID, sign, body) as RequestMessagePacket;

            if (callback != null)
            {
                var action = ActionFactory.CreateAction.Invoke(channelName, ContractID);
                action.Params = sendParams;
                action.Callback = callback;
                this.SendActions.Add(message.MessageID, action);
            }

            if (this.SendCallBack != null)
            {
                //当前不一定是主线程，，得添加到loom中
                //loom是在update内执行（不是立即）。。确保这里是顺序执行
                Loom.QueueOnMainThread(() =>
                {
                    SendCallBack.Invoke(channel.Pipeline, message, loadMask);
                    channel.Pipeline.WriteAndFlushAsync(message);
                });
            }
            else
            {
                channel.Pipeline.WriteAndFlushAsync(message);
            }
        }



        public BlowFish GetCrypt(string key)
        {
            if (crypts.TryGetValue(key, out BlowFish fish) == false)
            {
                fish = new BlowFish(key);
                crypts.Add(key, fish);
            }
            return fish;
        }

        public byte[] CreateBody(Dictionary<string, string> sendParams)
        {
            if (sendParams == null || sendParams.Count == 0)
            {
                return new byte[0];
            }

            List<string> sendarr = new List<string>();
            foreach (var param in sendParams)
            {
                sendarr.Add(param.Key + "=" + param.Value);
            }
            var body = String.Join("&", sendarr.ToArray());
            Debug.Log($"BODY:{body}");
            return Encoding.UTF8.GetBytes(body);
        }


    }
}
