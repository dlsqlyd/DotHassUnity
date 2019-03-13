
using DotHass.Unity;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using PureMVC.Patterns.Command;
using PureMVC.Patterns.Facade;

namespace DotHassUnity
{
    class RestartCommand : SimpleCommand, ICommand
    {

        public RestartCommand()
        {

        }


        public override void Execute(INotification note)
        {
            //重新注册facade。。主要是为了清除所有的proxy，command等
            App.ResetFacade();
            AppInstaller.RegisterFacadeCoreCommand();
        }



    }

}
