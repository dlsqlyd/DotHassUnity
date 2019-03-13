using DotHass.Unity;
using DotHass.Unity.Net;
using PureMVC.Interfaces;
using PureMVC.Patterns.Command;
using SDGame.UITools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace DotHassUnity
{
    public class PassportCommand : SimpleCommand, ICommand
    {
        private Transform screen;
        private AppSetting appSetting;
        private NetService net;

        public PassportCommand([Inject(Id = AppConst.MainId)]Transform screenManager,

            AppSetting appSetting
             , NetService net
            )
        {
            this.screen = screenManager;
            this.appSetting = appSetting;
            this.net = net;
        }

        public override async void Execute(INotification notification)
        {
            switch (notification.Name)
            {
                case NoticeConst.PassportEnter:
                    foreach (var option in appSetting.options)
                    {
                        switch (option.Type)
                        {
                            case DotHass.Unity.Net.ChannelType.TCP:
                                await net.ConnectTcp(option);
                                break;
                            case DotHass.Unity.Net.ChannelType.KCP:
                                await net.ConnectKcp(option);
                                break;
                            default:
                                break;
                        }
                    }
                    var loginScreen = screen.Find("PassportPanel");

                    // 注册Prxoy
                    Facade.RegisterProxyUseTName<PassportProxy>();

                    // 注册Mediator
                    Facade.RegisterOnceMediatorUseTName<PassportMediator>(loginScreen.GetComponent<UIControlData>());


                    break;

                case NoticeConst.PassportExit:

                    Facade.SendNotification(SceneConst.FlowScene, new SceneFlowEvent()
                    {
                        UnloadSceneName = AppConst.EnterSceneName,
                        LoadSceneName = AppConst.MainSceneName,
                        ExtraBindings = (container) =>
                        {
                            // todo：这里可以处理一定传值操作
                            // container.BindInstance(mainLoader);
                        },
                        CallBack = () =>
                        {
                            //Todo:移除mediator和不必要的command
                            Facade.RemoveProxy(PassportProxy.TypeName);
                            Facade.RemoveMediator(PassportMediator.TypeName);
                            Facade.RemoveCommand(NoticeConst.PassportEnter);
                            Facade.RemoveCommand(NoticeConst.PassportExit);
                        }
                    });
                    break;
                default:
                    break;
            }
        }
    }

}
