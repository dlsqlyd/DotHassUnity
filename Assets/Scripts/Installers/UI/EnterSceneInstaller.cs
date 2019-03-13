using UnityEngine;
using Zenject;
using DotHass.Unity;
using System;
using System.Threading.Tasks;
using SDGame.UITools;
using System.Collections.Generic;
using DotHassUnity;



namespace DotHassUnity
{
    public class EnterSceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            var canvas = this.transform.GetComponentInChildren<Canvas>(AppConst.RootName);
            UIInstaller.Install(Container, canvas);
        }
    }
}