using UnityEngine;
using Zenject;
using DotHass.Unity;


namespace DotHassUnity
{
    public class MainSceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            var canvas = this.transform.GetComponentInChildren<Canvas>(AppConst.RootName);
            UIInstaller.Install(Container, canvas);

        }
    }
}