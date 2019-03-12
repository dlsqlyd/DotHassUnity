using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HFramework
{
    static public class GameObjectExtension
    {

       

        #region Cloning

        /// <summary>
        /// Clones an object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T Clone<T>(this T obj) where T : MonoBehaviour
        {
            return GameObject.Instantiate<T>(obj);
        }

        /// <summary>
        /// Clones an object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static List<T> Clone<T>(this T obj, int count) where T : MonoBehaviour
        {
            var list = new List<T>();

            for (int i = 0; i < count; i++)
            {
                list.Add(obj.Clone<T>());
            }

            return list;
        }

        #endregion

        #region Typesafe methods for scheduling

        /// <summary>
        /// Invokes the given action after the given amount of time.
        /// </summary>
        public static Coroutine Invoke(this MonoBehaviour monoBehaviour, Action action, float time)
        {
            return monoBehaviour.StartCoroutine(InvokeImpl(action, time));
        }

        private static IEnumerator InvokeImpl(Action action, float time)
        {
            yield return new WaitForSeconds(time);

            action();
        }

        /// <summary>
        /// Invokes the given action after the given amount of time, and repeats the 
        /// action after every repeatTime seconds.
        /// </summary>
        public static Coroutine InvokeRepeating(this MonoBehaviour monoBehaviour, Action action, float time, float repeatTime)
        {
            return monoBehaviour.StartCoroutine(InvokeRepeatingImpl(action, time, repeatTime));
        }

        private static IEnumerator InvokeRepeatingImpl(Action action, float time, float repeatTime)
        {
            yield return new WaitForSeconds(time);

            while (true)
            {
                action();
                yield return new WaitForSeconds(repeatTime);
            }
        }

       

        #endregion

        #region tween
        public static IEnumerator TweenImpl<T>(
            T start,
            T finish,
            float totalTime,
            Func<T, T, float, T> lerp,
            Action<T> action,
            Func<float> deltaTime)
        {
            float time = 0;
            float t = 0;

            while (t < 1)
            {
                var current = lerp(start, finish, t);
                action(current);

                time += deltaTime();
                t = time / totalTime;

                yield return null;
            }

            action(finish);
        }

        public static Coroutine Tween<T>(this MonoBehaviour monoBehaviour, T start, T finish, float totalTime, Func<T, T, float, T> lerp, Action<T> action)
        {
            return Tween(monoBehaviour, start, finish, totalTime, lerp, action, () => Time.deltaTime);
        }

        public static Coroutine Tween<T>(
            this MonoBehaviour monoBehaviour,
            T start,
            T finish,
            float totalTime,
            Func<T, T, float, T> lerp,
            Action<T> action,
            Func<float> deltaTime)
        {
            return monoBehaviour.StartCoroutine(TweenImpl(start, finish, totalTime, lerp, action, deltaTime));
        }
        #endregion


        #region Component


        private static Transform FindChildRecursively(Transform target, string childName)
        {
            if (target.name == childName) return target;

            for (var i = 0; i < target.childCount; ++i)
            {
                var result = FindChildRecursively(target.GetChild(i), childName);

                if (result != null) return result;
            }

            return null;
        }

        public static Transform Find(this Component component, string childName, bool recursive)
        {
            if (recursive) return component.Find(childName);

            return FindChildRecursively(component.transform, childName);
        }

        public static Transform Find(this Component component, string childName)
        {
            return component.transform.Find(childName);
        }

        public static Transform Find(this GameObject go, string childName)
        {
            return go.transform.Find(childName);
        }

        /// <summary>
        /// 先查找gameobject,再获取组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="component"></param>
        /// <param name="childName"></param>
        /// <returns></returns>

        public static T GetComponentInChildren<T>(this Component component, string childName)
        {
            return component.Find(childName).GetComponent<T>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="go"></param>
        /// <param name="childName"></param>
        /// <returns></returns>

        public static T GetComponentInChildren<T>(this GameObject go, string childName)
        {
            return go.transform.GetComponentInChildren<T>(childName);
        }

        /// <summary>
        /// Gets the or add component.
        /// </summary>
        /// <returns>The or add component.</returns>
        /// <param name="go">Go.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static T GetOrAddComponent<T>(this Component component) where T : Component
        {
            return GetOrAddComponent<T>(component.gameObject);
        }
        /// <summary>
        /// Gets the or add component.
        /// </summary>
        /// <returns>The or add component.</returns>
        /// <param name="go">Go.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static T GetOrAddComponent<T>(this GameObject go) where T : Component
        {
            T ret = go.GetComponent<T>();
            if (null == ret)
                ret = go.AddComponent<T>();
            return ret;
        }

        /// <summary>
        /// Gets the or add component.
        /// </summary>
        /// <returns>The or add component.</returns>
        /// <param name="go">Go.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static Component GetOrAddComponent(this GameObject go,Type type) 
        {
            var  ret = go.GetComponent(type);
            if (null == ret)
                ret = go.AddComponent(type);
            return ret;
        }


        /// <summary>
        /// Similar to FindObjectsOfType, except that it looks for components
        /// that implement a specific interface.
        /// </summary>
        public static List<I> FindObjectsOfInterface<I>() where I : class
        {
            var monoBehaviours = GameObject.FindObjectsOfType<MonoBehaviour>();

            return monoBehaviours.Select(behaviour => behaviour.GetComponent(typeof(I))).OfType<I>().ToList();
        }


        /// <summary>
        /// Gets a component of the given type, or fail if no such component is attached to the given component.
        /// </summary>
        /// <typeparam name="T">The type of component to get.</typeparam>
        /// <param name="thisComponent">The component to check.</param>
        /// <returns>A component of type T attached to the given component if it exists.</returns>
        /// <exception cref="InvalidOperationException">When the no component of the required type exist on the given component.</exception>
        //TODO Implement variants

        //Make this a instance method too
        public static T GetRequiredComponent<T>(this Component thisComponent) where T : Component
        {
            var retrievedComponent = thisComponent.GetComponent<T>();

            if (retrievedComponent == null)
            {
                throw new InvalidOperationException(string.Format("GameObject \"{0}\" ({1}) does not have a component of type {2}", thisComponent.name, thisComponent.GetType(), typeof(T)));
            }

            return retrievedComponent;
        }

        public static T GetRequiredComponentInChildren<T>(this Component thisComponent) where T : Component
        {
            var retrievedComponent = thisComponent.GetComponentInChildren<T>();

            if (retrievedComponent == null)
            {
                throw new InvalidOperationException(string.Format("GameObject \"{0}\" ({1}) does not have a child with component of type {2}", thisComponent.name, thisComponent.GetType(), typeof(T)));
            }

            return retrievedComponent;
        }


        /// <summary>
        /// Gets an attached component that implements the interface of the type parameter.
        /// </summary>
        /// <typeparam name="TInterface">The type of the t interface.</typeparam>
        /// <param name="thisComponent">The this component.</param>
        /// <returns>TInterface.</returns>
        public static TInterface GetInterfaceComponent<TInterface>(this Component thisComponent) where TInterface : class
        {
            return thisComponent.GetComponent(typeof(TInterface)) as TInterface;
        }


        /// <summary>
        /// Finds a component of the type T in on the same object, or on a child down the hierarchy. This method also works
        /// in the editor and when the game object is inactive.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="component">The component.</param>
        /// <returns>T.</returns>
        public static T GetComponentInChildrenAlways<T>(this Component component) where T : Component
        {
            foreach (var child in component.transform.SelfAndAllChildren())
            {
                var componentInChild = child.GetComponent<T>();

                if (componentInChild != null)
                {
                    return componentInChild;
                }
            }

            return null;
        }

        /**
			Finds all components of the type T on the same object and on a children down the hierarchy. This method also works
			in the editor and when the game object is inactive.

			@version_e_1_1
		*/

        public static T[] GetComponentsInChildrenAlways<T>(this Component component) where T : Component
        {
            var components = new List<T>();

            foreach (var child in component.transform.SelfAndAllChildren())
            {
                var componentsInChild = child.GetComponents<T>();

                if (componentsInChild != null)
                {
                    components.AddRange(componentsInChild);
                }
            }

            return components.ToArray();
        }
        #endregion
    }
}