
using PureMVC.Interfaces;
using PureMVC.Patterns;
using PureMVC.Patterns.Command;



namespace DotHassUnity
{
    public class ShutdownCommand : SimpleCommand, ICommand
    {
        public override void Execute(INotification note)
        {
        }
    }
}
