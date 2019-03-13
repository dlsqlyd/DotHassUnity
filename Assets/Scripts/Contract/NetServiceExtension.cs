using DotHass.Unity.Net;
using DotHass.Unity.Net.Abstractions;
using DotHass.Unity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;



public static class NetServiceExtension
{

    /// <summary>
    /// 当callback为null的时候。不保存该消息。。直接发送给服务器然后就不管了
    /// </summary>
    /// <param name="net"></param>
    /// <param name="ContractID"></param>
    /// <param name="sendParams"></param>
    /// <param name="callback"></param>
    public static void Send(this NetService net, int ContractID, Dictionary<string, string> sendParams)
    {
        net.Send("Game", ContractID, sendParams, null, false);
    }

    //需要等待和返回值的才使用这个
    public static async Task<object> Send(this NetService net, short ContractID, Dictionary<string, string> sendParams, Action<ErrorInfo> errorCallback = null, bool loadMask = true)
    {
        return await net.Send("Game", ContractID, sendParams, errorCallback, loadMask);
    }

    //需要等待和返回值的才使用这个
    public static async Task<T> Send<T>(this NetService net, short ContractID, Dictionary<string, string> actionParam, Action<ErrorInfo> errorCallback = null, bool loadMask = true)
    {
        var v = await net.Send("Game", ContractID, actionParam, errorCallback, loadMask);
        return JsonConvert.DeserializeObject<T>(v.ToString());
    }


}