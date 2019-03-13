using DotHass.Unity;
using PureMVC.Interfaces;
using PureMVC.Patterns.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DotHassUnity
{

    public class EnterSceneCommand : SimpleCommand, ICommand
    {

        public override void Execute(INotification notification)
        {
            var scene = notification.Body as IScene;

            Facade.SendNotification(NoticeConst.Startup);
            //1.为了单个场景可以单独使用，所以每个场景都需要执行startup命令
            //2 一定要移除，避免切换场景重复执行startup命令。
            Facade.RemoveCommand(NoticeConst.Startup);


            switch (scene.Name)
            {
                case AppConst.EnterSceneName:


                    Facade.RegisterCommandForMultipleNotice<PassportCommand>(NoticeConst.PassportEnter, NoticeConst.PassportExit);

                    Facade.SendNotification(NoticeConst.PassportEnter);
                    break;
                case AppConst.MainSceneName:

                    Facade.RegisterProxyUseTName<RoleProxy>();

                    Facade.RegisterCommandForMultipleNotice<HomeCommand>(NoticeConst.HomeEnter, NoticeConst.HomeExit);


                    Facade.SendNotification(NoticeConst.HomeEnter);

                    break;

                default:
                    break;
            }
        }



    }

}
