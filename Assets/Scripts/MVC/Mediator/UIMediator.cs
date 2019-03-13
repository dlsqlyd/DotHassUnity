using PureMVC.Interfaces;
using PureMVC.Patterns.Mediator;
using SDGame.UITools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DotHassUnity
{
    public class UIMediator : Mediator, IBindableUI
    {
        #region Variable

        #endregion

        #region Proxy

        #endregion

        public UIMediator(string mediatorName, UIControlData ctrlData) : base(mediatorName, null)
        {
            if (ctrlData != null)
            {
                ctrlData.BindDataTo(this);
            }
        }

        public override string[] ListNotificationInterests()
        {
            return new string[0];
        }

        public override void HandleNotification(INotification notification)
        {
        }

        public override void OnRegister()
        {

        }

        public override void OnRemove()
        {
        }

    }
}
