using PureMVC.Interfaces;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace HFramework
{
    /// <summary>
    /// 场景入口类
    /// 主动加载app
    /// </summary>
    public class HSceneContext : SceneContext, IScene
    {
        public string Name { get; set; }
        public bool StartUpComplete { get; set; }

        /// <summary>
        /// In this class we override the start method so that we can trigger the kernel to load if its not already.
        /// </summary>
        /// <returns></returns>
        protected virtual void Start()
        {
            var sceneservice = Container.Resolve<ISceneService>();
            StartCoroutine(sceneservice.StartUpScene(this));
        }

        public virtual IEnumerator Enter()
        {
            //确定Container初始化完成
            while (Initialized == false)
            {
                yield return null;
            }
            App.SceneContainer = Container;
            App.Facade.SendNotification(SceneConst.EnterScene, this);
            yield return null;
        }

        /// <summary>
        /// 切换场景后将CurrentContainer设为null
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerator Exit()
        {
            App.Facade.SendNotification(SceneConst.ExitScene, this);
            yield return null;
        }


        public virtual void OnDestroy()
        {

        }

    }
}
