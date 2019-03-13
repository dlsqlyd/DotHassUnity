using DotHass.Unity;
using PureMVC.Interfaces;
using PureMVC.Patterns.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotHassUnity
{
    public class ExitSceneCommand : SimpleCommand, ICommand
    {
        public override void Execute(INotification notification)
        {
            var scene = notification.Body as IScene;



            switch (scene.Name)
            {
                case AppConst.EnterSceneName:

                    break;
                case AppConst.MainSceneName:

                    break;
                default:
                    break;
            }
        }
    }
}
