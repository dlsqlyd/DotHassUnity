using System;
using System.Collections.Generic;
using DotHassUnity;
using DotHass.Unity;
using DotHass.Unity.Net;
using DotHass.Unity.Net.Message;
using PureMVC.Interfaces;
using PureMVC.Patterns.Facade;
using UnityEngine;
using Zenject;


namespace DotHassUnity
{
    public class AppInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            //一些初始化的东西
            ActionFactory.CreateAction = ActionIDCreateActionFactory.CreateAction;

            RegisterFacadeCoreCommand();

            //绑定DotHass.Unity的service

            Container.BindInterfacesAndSelfTo<AssetService>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<CoroutineService>().FromNewComponentOn(gameObject).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<SceneService>().AsSingle().NonLazy();

            Container.BindInterfacesAndSelfTo<AudioService>().AsSingle().OnInstantiated<AudioService>((ctx, audio) =>
            {
                audio.globalVolume = ctx.Container.Resolve<AppSetting>().globalVolume;
            }).NonLazy();
            Container.BindInterfacesAndSelfTo<PoolService>().AsSingle();
            Container.BindInterfacesAndSelfTo<SaveService>().AsSingle();
            Container.BindInterfacesAndSelfTo<NetService>().AsSingle();



            ValueObjectInstaller.Install(Container);
        }

        public static void RegisterFacadeCoreCommand()
        {
            App.Facade.RegisterCommand<FlowSceneCommand>(SceneConst.FlowScene);
            App.Facade.RegisterCommand<EnterSceneCommand>(SceneConst.EnterScene);
            App.Facade.RegisterCommand<ExitSceneCommand>(SceneConst.ExitScene);
            App.Facade.RegisterCommand<StartupCommand>(NoticeConst.Startup);
            App.Facade.RegisterCommand<RestartCommand>(NoticeConst.Restart);
            App.Facade.RegisterCommand<ShutdownCommand>(NoticeConst.Shutdown);
        }

        void InstallExecutionOrder()
        {
            // In many cases you don't need to worry about execution order,
            // however sometimes it can be important
            // If for example we wanted to ensure that AsteroidManager.Initialize
            // always gets called before GameController.Initialize (and similarly for Tick)
            // Then we could do the following:



            // Note that they will be disposed of in the reverse order given here
        }
    }
}