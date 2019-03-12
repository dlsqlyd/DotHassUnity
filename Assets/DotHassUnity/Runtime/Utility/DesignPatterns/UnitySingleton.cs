using System;
using UnityEngine;

namespace DotHass.Unity
{
    /// <summary>
    ///  A singleton implementation pattern
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class UnitySingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        protected static T _Instance;

        public static T Instance
        {
            get
            {
                if (_Instance == null)
                {
                    if (!UnityEngine.Application.isPlaying) return null;
                    _Instance = FindObjectOfType(typeof(T)) as T;
                    if (_Instance == null)
                    {
                        GameObject obj = new GameObject(typeof(T).Name);
                        //obj.hideFlags = HideFlags.HideAndDontSave;
                        _Instance = obj.AddComponent(typeof(T)) as T;
                    }
                }

                return _Instance;
            }
        }

        private void Awake()
        {
            if (_Instance == null)
            {
                _Instance = this as T;
            }
            else if (_Instance != this)
            {
                Destroy(this); //销毁新实例化的gameObject,保证单例是第一个,而且是唯一的
                return;
            }
        }
    }
}