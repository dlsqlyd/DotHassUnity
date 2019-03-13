using DotHass.Unity;
using DotHass.Unity.Net;
using Newtonsoft.Json.Linq;
using PureMVC.Interfaces;
using PureMVC.Patterns.Proxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace DotHassUnity
{
    public class PassportProxy : Proxy
    {
        public static string TypeName { get; } = typeof(PassportProxy).Name;

        private NetService net;

        private SaveService save;
        private UserVo userVo;


        public PassportProxy(NetService netService, SaveService saveService, UserVo vo) : base(TypeName, null)
        {
            this.net = netService;
            this.userVo = vo;
            save = saveService;
        }

        public async void Login(PassportVo tryLogin)
        {
            this.userVo = await this.net.Send<UserVo>(ActionIDDefine.Login, new Dictionary<string, string> {
                {"Pid",tryLogin.username},
                {"Pwd",tryLogin.password}
            }, (ErrorInfo e) =>
            {
                App.Facade.SendNotification(NoticeConst.LOGIN_FAILED, e);
            });

            save.Save<string>("pid", tryLogin.username);
            Facade.SendNotification(NoticeConst.LOGIN_SUCCESS);
        }


        public async void Reg(PassportVo tryReg)
        {
            this.userVo = await this.net.Send<UserVo>(ActionIDDefine.Reg, new Dictionary<string, string> {
                {"Pid",tryReg.username},
                {"Pwd",tryReg.password }
            }, (ErrorInfo e) =>
            {
                App.Facade.SendNotification(NoticeConst.REG_FAILED, e);
            });
            save.Save<string>("pid", tryReg.username);
            Facade.SendNotification(NoticeConst.REG_SUCCESS);
        }
    }
}
