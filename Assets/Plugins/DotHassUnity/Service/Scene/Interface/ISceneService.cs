using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using Zenject;

namespace HFramework
{
    public interface ISceneService
    {
        IEnumerator UnloadSceneAsync(string name, bool HierarchyUnloadScene = false);
        IEnumerator LoadSceneAsync(string loadSceneName, LoadSceneMode sceneMode, Action<DiContainer> extraBindings);
        IEnumerator StartUpScene(IScene sceneRoot);
    }
}
