using PureMVC.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;

namespace HFramework
{

    public static class FacadeExtensions
    {
        public static DiContainer Container
        {
            get
            {
                return App.SceneContainer;
            }
        }

        #region IProxy
        public static IProxy RegisterProxy<T>(this IFacade facade) where T : IProxy
        {
            var proxy = Container.Instantiate<T>();
            facade.RegisterProxy(proxy);
            return proxy;
        }

        public static IProxy RegisterProxy<T>(this IFacade facade, params object[] list) where T : IProxy
        {
            var proxy = Container.Instantiate<T>(list);
            facade.RegisterProxy(proxy);
            return proxy;
        }

        public static IProxy RegisterProxyUseTName<T>(this IFacade facade) where T : IProxy
        {
            var typeName = typeof(T).Name;
            if (facade.HasProxy(typeName))
            {
                return facade.RetrieveProxy(typeName);
            }
            return facade.RegisterProxy<T>();
        }

        public static IProxy RegisterProxyUseTName<T>(this IFacade facade, params object[] list) where T : IProxy
        {
            var typeName = typeof(T).Name;
            if (facade.HasProxy(typeName))
            {
                return facade.RetrieveProxy(typeName);
            }
            return facade.RegisterProxy<T>(list);
        }
        #endregion





        #region IMediator
        public static IMediator RegisterMediator<T>(this IFacade facade) where T : IMediator
        {
            var mediator = Container.Instantiate<T>();
            facade.RegisterMediator(mediator);
            return mediator;
        }

        public static IMediator RegisterMediator<T>(this IFacade facade, params object[] list) where T : IMediator
        {
            var mediator = Container.Instantiate<T>(list);
            facade.RegisterMediator(mediator);
            return mediator;
        }

        public static IMediator RegisterOnceMediatorUseTName<T>(this IFacade facade) where T : IMediator
        {
            var typeName = typeof(T).Name;
            if (facade.HasMediator(typeName))
            {
                return facade.RetrieveMediator(typeName);
            }
            return facade.RegisterMediator<T>();
        }

        public static IMediator RegisterOnceMediatorUseTName<T>(this IFacade facade, params object[] list) where T : IMediator
        {
            var typeName = typeof(T).Name;
            if (facade.HasMediator(typeName))
            {
                return facade.RetrieveMediator(typeName);
            }
            return facade.RegisterMediator<T>(list);
        }
        #endregion


        #region command
        /// <summary>
        /// 命令每次被通知都会被实例化一次。
        /// controller不会保存command实例。
        /// 只是保存一个command工厂
        /// 而且尽量不要再命令里返回单例
        /// 
        /// ----------------------------------
        /// 同一个通知。。command不会像往常的事件一样被注册多次。
        /// 一个通知只有一个回调。新注册的会代替后注册的。
        /// 如果想再同一个通知执行多次可以使用MacroCommand
        /// 但是注意由于分割线上面的原因。而且MacroCommand的子命令执行后会删除
        /// 所以MacroCommand一定不要注册单利。。否则其子命令只会执行一次
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="facade"></param>
        /// <param name="notificationName"></param>
        public static void RegisterCommand<T>(this IFacade facade, string notificationName) where T : ICommand
        {
            facade.RegisterCommand(notificationName, () => Container.Instantiate<T>());
        }

        /// <summary>
        /// 命令每次都会被实例化一次
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="facade"></param>
        /// <param name="notificationName"></param>
        public static void RegisterCommand<T>(this IFacade facade, string notificationName, params object[] list) where T : ICommand
        {
            facade.RegisterCommand(notificationName, () => Container.Instantiate<T>(list));
        }


        public static void RegisterCommandForMultipleNotice<T>(this IFacade facade, params string[] notifications) where T : ICommand
        {
            foreach (var notificationName in notifications)
            {
                facade.RegisterCommand<T>(notificationName);
            }
        }


        public static void RegisterCommandForMultipleNotice<T>(this IFacade facade, object[] list, params string[] notifications) where T : ICommand
        {
            foreach (var notificationName in notifications)
            {
                facade.RegisterCommand<T>(notificationName, list);
            }
        }
        #endregion

    }
}
