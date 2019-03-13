using DotHass.Unity;
using PureMVC.Interfaces;
using PureMVC.Patterns.Command;
using SDGame.UITools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace DotHassUnity
{
    public class HomeCommand : SimpleCommand, ICommand
    {
        private Transform screen;

        private RoleProxy roleProxy
        {
            get
            {
                return Facade.RetrieveProxy(RoleProxy.TypeName) as RoleProxy;
            }
        }


        public HomeCommand([Inject(Id = AppConst.MainId)]Transform screenManager)
        {
            this.screen = screenManager;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="notification"></param>
        public override void Execute(INotification notification)
        {
            switch (notification.Name)
            {
                case NoticeConst.HomeEnter:
                    var homePanel = screen.Find("HomePanel");
                    var control = homePanel.GetComponent<UIControlData>();
                    Facade.RegisterOnceMediatorUseTName<HomeMediator>(control);
                    break;

                case NoticeConst.HomeExit:
                    //Todo:移除mediator和不必要的command
                    Facade.RemoveMediator(HomeMediator.TypeName);
                    Facade.RemoveCommand(NoticeConst.HomeEnter);
                    Facade.RemoveCommand(NoticeConst.HomeExit);
                    break;
                default:
                    break;
            }
        }
    }
}
