using PureMVC.Patterns;
using PureMVC.Interfaces;
using UnityEngine;
using PureMVC.Patterns.Command;
using DotHass.Unity;
using PureMVC.Patterns.Facade;

namespace DotHassUnity
{
    public class StartupCommand : MacroCommand, ICommand
    {
        //执行该命令的时候会执行其中的子命令方法
        protected override void InitializeMacroCommand()
        {
            AddSubCommand(() => new BootstrapCommands());
            AddSubCommand(() => new BootstrapProxys());
            AddSubCommand(() => new BootstrapMediators());
        }

        public override void Execute(INotification note)
        {
            base.Execute(note);
            Screen.orientation = ScreenOrientation.Portrait;
        }
    }
}
