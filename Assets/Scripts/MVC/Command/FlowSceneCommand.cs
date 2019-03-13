using PureMVC.Interfaces;
using PureMVC.Patterns.Command;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using Zenject;
using DotHass.Unity;
using UnityEngine;
namespace DotHassUnity
{
    public class FlowSceneCommand : SimpleCommand, ICommand
    {
        private ISceneService service;
        private ICoroutine conroutine;

        public FlowSceneCommand(ISceneService service, ICoroutine monoDriver)
        {
            this.service = service;
            this.conroutine = monoDriver;
        }

        public override void Execute(INotification note)
        {
            var flowBody = note.Body as SceneFlowEvent;
            conroutine.StartCoroutine(FlowScene(flowBody));
        }


        public IEnumerator FlowScene(SceneFlowEvent flowBody)
        {
            if (flowBody.SceneMode == LoadSceneMode.Additive)
            {
                //不会黑屏..但是在卸载前会有两个场景共存..内存会很大
                yield return service.LoadSceneAsync(flowBody.LoadSceneName, flowBody.SceneMode, flowBody.ExtraBindings);

                if (!string.IsNullOrEmpty(flowBody.UnloadSceneName))
                {
                    yield return service.UnloadSceneAsync(flowBody.UnloadSceneName, true);
                }
            }
            else if (flowBody.SceneMode == LoadSceneMode.Single)
            {
                //如果是Single会在加载之前会卸载其他所有的场景,为了保证上个场景全部清理完毕后再销毁..所以先执行
                //但这个方法有个问题..切换场景会短暂黑屏
                if (!string.IsNullOrEmpty(flowBody.UnloadSceneName))
                {
                    yield return service.UnloadSceneAsync(flowBody.UnloadSceneName);
                    //因为是先卸载的..所以这个可以是null..
                    App.SceneContainer = null;
                }
                yield return service.LoadSceneAsync(flowBody.LoadSceneName, flowBody.SceneMode, flowBody.ExtraBindings);
            }
            if (flowBody.CallBack != null)
            {
                flowBody.CallBack.Invoke();
            }
            //立即进行垃圾回收
            GC.Collect();
            GC.WaitForPendingFinalizers();//挂起当前线程，直到处理终结器队列的线程清空该队列为止
            GC.Collect();
        }
    }

    public class SceneFlowEvent
    {
        public LoadSceneMode SceneMode = LoadSceneMode.Additive;
        public string UnloadSceneName;
        public string LoadSceneName;
        public Action<DiContainer> ExtraBindings;
        public Action CallBack;//注意不要在这里操作gameobject..旧场景已经销毁
    }
}
