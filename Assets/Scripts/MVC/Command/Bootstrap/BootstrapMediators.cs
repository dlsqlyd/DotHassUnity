using PureMVC.Interfaces;
using PureMVC.Patterns.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotHassUnity
{
    class BootstrapMediators : SimpleCommand, ICommand
    {
        public override void Execute(INotification note)
        {

        }
    }
}
