using DotHassUnity;
using DotHass.Unity;
using UnityEngine;
using Zenject;


namespace DotHassUnity
{
    public class UIInstaller : Installer<Canvas, UIInstaller>
    {
        private Canvas canvas;

        public UIInstaller(Canvas canvas)
        {
            this.canvas = canvas;
        }


        public override void InstallBindings()
        {
            Container.Bind<Canvas>().WithId(AppConst.RootId).FromInstance(this.canvas);
            Container.Bind<Transform>().WithId(AppConst.MainId).FromInstance(this.canvas.GetComponentInChildren<Transform>(AppConst.MainName));
            Container.Bind<Transform>().WithId(AppConst.PopId).FromInstance(this.canvas.GetComponentInChildren<Transform>(AppConst.PopName));
            Container.Bind<Transform>().WithId(AppConst.FloatingId).FromInstance(this.canvas.GetComponentInChildren<Transform>(AppConst.FloatingName));



        }
    }
}
