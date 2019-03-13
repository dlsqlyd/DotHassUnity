using DotHass.Unity;
using DotHass.Unity.Net;
using PureMVC.Core;
using PureMVC.Interfaces;
using PureMVC.Patterns.Mediator;
using SDGame.UITools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;
using static UnityEngine.RectTransform;

namespace DotHassUnity
{
    public class HomeMediator : UIMediator
    {
        #region 控件绑定变量声明，自动生成请勿手改
        [ControlBinding]
        private Text name;
        [ControlBinding]
        private Text gold;
        [ControlBinding]
        private Text money;
        [ControlBinding]
        private Text gang;

        #endregion





        #region Variable
        public static string TypeName { get; } = typeof(HomeMediator).Name;

        private NetService net;


        #endregion

        #region Proxy
        private RoleProxy roleProxy
        {
            get
            {
                return Facade.RetrieveProxy(RoleProxy.TypeName) as RoleProxy;
            }
        }

        #endregion

        public HomeMediator(NetService netService, UIControlData viewComponent) : base(TypeName, viewComponent)
        {
            this.net = netService;
        }


        /// <summary>
        /// Called by the View when the Mediator is registered
        /// </summary>
        public override async void OnRegister()
        {
            var vo = await this.net.Send<RoleVo>(ActionIDDefine.RoleInfo, null);

            name.text = "Id:" + vo.roleid.ToString();

            gold.text = "金币:" + vo.gold.ToString();


            money.text = "金钱:" + vo.money.ToString();

            gang.text = "门派:" + vo.gang.Name;
        }


        /// <summary>
        /// Called by the View when the Mediator is removed
        /// </summary>
        public override void OnRemove()
        {
        }


        public override string[] ListNotificationInterests()
        {
            return new string[] {
            };
        }

        /// <summary>
        /// Handle <c>INotification</c>s.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         Typically this will be handled in a switch statement,
        ///         with one 'case' entry per <c>INotification</c>
        ///         the <c>Mediator</c> is interested in.
        ///     </para>
        /// </remarks>
        /// <param name="notification"></param>
        public override void HandleNotification(INotification notification)
        {
        }

    }
}
