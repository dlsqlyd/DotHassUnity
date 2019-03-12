using PureMVC.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace HFramework
{

    public class SceneService : ISceneService
    {

        private ZenjectSceneLoader zenjectSceneLoader;
        private Queue<SceneQueueItem> _scenesQueue;

        public Queue<SceneQueueItem> ScenesQueue
        {
            get { return _scenesQueue ?? (_scenesQueue = new Queue<SceneQueueItem>()); }
        }

        public List<IScene> LoadedScenes
        {
            get { return _loadedScenes ?? (_loadedScenes = new List<IScene>()); }
        }

        private List<IScene> _loadedScenes;


        public SceneService(ZenjectSceneLoader zenjectSceneLoader)
        {
            this.zenjectSceneLoader = zenjectSceneLoader;
        }

        #region laod scene

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="loadSceneMode"></param>
        /// <param name="extraBindings"></param>
        /// <param name="startup">如果为true,加载成功后则自动startup..如果为false.得用户手动startup</param>
        /// <returns></returns>
        public IEnumerator LoadSceneAsync(string name, LoadSceneMode loadSceneMode, Action<DiContainer> extraBindings)
        {
            switch (loadSceneMode)
            {
                case LoadSceneMode.Single:
                    ScenesQueue.Enqueue(new SceneQueueItem()
                    {
                        Loader = InstantiateSceneAsync(name, extraBindings),
                        Name = name,
                    });
                    break;
                case LoadSceneMode.Additive:
                    ScenesQueue.Enqueue(new SceneQueueItem()
                    {
                        Loader = InstantiateSceneAsyncAdditively(name, extraBindings),
                        Name = name,
                    });
                    break;
            }
            yield return ExecuteLoadAsync();
        }

        public IEnumerator InstantiateSceneAsync(string sceneName, Action<DiContainer> extraBindings)
        {
            yield return this.zenjectSceneLoader.LoadSceneAsync(sceneName, LoadSceneMode.Single, extraBindings);
        }

        public IEnumerator InstantiateSceneAsyncAdditively(string sceneName, Action<DiContainer> extraBindings)
        {
            if (LoadedScenes.Any(p => p.Name == sceneName))
            {
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
                yield break;
            }
            yield return this.zenjectSceneLoader.LoadSceneAsync(sceneName, LoadSceneMode.Additive, extraBindings);
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
        }

        protected IEnumerator ExecuteLoadAsync()
        {
            foreach (var sceneQueeItem in ScenesQueue.ToArray())
            {
                yield return sceneQueeItem.Loader;
            }
        }

        #endregion laod scene

        #region unload scene

        public IEnumerator UnloadSceneAsync(string name, bool HierarchyUnloadScene = false)
        {
            var sceneRoot = LoadedScenes.FirstOrDefault(s => s.Name == name);
            if (sceneRoot == null)
            {
                yield break;
            }
            var sceneGo = (sceneRoot as MonoBehaviour).gameObject;
            var LastSceneName = sceneRoot.Name;
            UnRegisterScene(sceneRoot);
            yield return sceneRoot.Exit();
            sceneGo.SetActive(false);
            GameObject.Destroy(sceneGo);
            if (HierarchyUnloadScene == true)
            {
                yield return SceneManager.UnloadSceneAsync(LastSceneName);
            }
        }

        #endregion unload scene

        #region 设置场景

        public IEnumerator StartUpScene(IScene sceneRoot)
        {
            if (sceneRoot.StartUpComplete == true)
            {
                yield break;
            }
            sceneRoot.StartUpComplete = true;
            var sceneName = SceneManager.GetActiveScene().name;
            RegisterScene(sceneRoot, sceneName);
            yield return sceneRoot.Enter();
        }

        private void RegisterScene(IScene sceneRoot, string sceneName)
        {
            sceneRoot.Name = sceneName;
            LoadedScenes.Add(sceneRoot);
        }

        private void UnRegisterScene(IScene sceneRoot)
        {
            LoadedScenes.Remove(sceneRoot);
        }

        #endregion 设置场景


        public static IScene GetActiveScene()
        {
            return GetScene(SceneManager.GetActiveScene());
        }
        public static IScene GetScene(string sceneName)
        {
            return GetScene(SceneManager.GetSceneByName(sceneName));
        }
        public static IScene GetScene(Scene scene)
        {
            GameObject[] p = scene.GetRootGameObjects();
            IScene hscene = null;
            foreach (var item in p)
            {
                hscene = item.GetComponent(typeof(IScene)) as IScene;
                if (hscene != null)
                {
                    break;
                }
            }
            return hscene;
        }
    }


    public struct SceneQueueItem
    {
        public string Name { get; set; }
        public IEnumerator Loader { get; set; }
    }

}
