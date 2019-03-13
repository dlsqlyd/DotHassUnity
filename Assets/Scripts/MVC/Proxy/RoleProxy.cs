using DotHass.Unity;
using PureMVC.Patterns.Proxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotHass.Unity.Net;
namespace DotHassUnity
{
    public class RoleProxy : Proxy
    {
        public static string TypeName { get; } = typeof(RoleProxy).Name;

        private NetService net;

        public RoleVo Vo
        {
            get
            {
                return Data as RoleVo;
            }
        }


        public RoleProxy(NetService netService, RoleVo roleVo) : base(TypeName, roleVo)
        {
            this.net = netService;
        }



    }
}
