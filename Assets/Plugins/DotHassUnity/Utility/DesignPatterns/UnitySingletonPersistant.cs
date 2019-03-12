using UnityEngine;

namespace HFramework
{
    /// <summary>
    ///  A singleton implementation pattern that is persistant across scene changes
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class UnitySingletonPersistant<T> : UnitySingleton<T> where T : MonoBehaviour
    {
        protected  virtual void Start()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}