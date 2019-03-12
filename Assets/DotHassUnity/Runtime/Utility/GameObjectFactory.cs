using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DotHass.Unity
{
    public static class GameObjectFactory
    {

        #region Instantiate
        /// <summary>
        /// Instantiates an object at the 
        /// given position in the given orientation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="prefab">The prefab to instantiate.</param>
        /// <param name="position">The position.</param>
        /// <param name="rotation">The rotation.</param>
        /// <returns>T.</returns>
        public static T Instantiate<T>(T prefab, Vector3 position, Quaternion rotation) where T : Component
        {
            var newObj = GameObject.Instantiate<T>(prefab);

            newObj.transform.position = position;
            newObj.transform.rotation = rotation;

            return newObj;
        }


        /// <summary>
        /// Instantiates a prefab and attaches it to the given root. 
        /// </summary>
        public static T Instantiate<T>(T prefab, GameObject root) where T : Component
        {
            var newObj = (T)UnityEngine.Object.Instantiate(prefab);
            newObj.transform.parent = root.transform;
            newObj.transform.ResetLocal();

            return newObj;
        }

        /// <summary>
        /// Instantiates a prefab, attaches it to the given root, and
        /// sets the local position and rotation.
        /// </summary>
        public static T Instantiate<T>(T prefab, GameObject root, Vector3 localPosition, Quaternion localRotation) where T : Component
        {
            var newObj = GameObject.Instantiate<T>(prefab);

            newObj.transform.parent = root.transform;

            newObj.transform.localPosition = localPosition;
            newObj.transform.localRotation = localRotation;
            newObj.transform.ResetScale();

            return newObj;
        }


        /// <summary>
        /// Instantiates a prefab.
        /// </summary>
        /// <param name="prefab">The object.</param>
        /// <returns>GameObject.</returns>
        public static GameObject Instantiate(GameObject prefab)
        {
            return (GameObject)UnityEngine.Object.Instantiate(prefab);
        }


        /// <summary>
        /// Instantiates the specified prefab.
        /// </summary>
        public static GameObject Instantiate(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            var newObj = (GameObject)UnityEngine.Object.Instantiate(prefab, position, rotation);

            return newObj;
        }


        /// <summary>
        /// Instantiates a prefab and parents it to the root.
        /// </summary>
        /// <param name="prefab">The prefab.</param>
        /// <param name="root">The root.</param>
        /// <returns>GameObject.</returns>
        public static GameObject Instantiate(GameObject prefab, GameObject root)
        {
            var newObject = (GameObject)UnityEngine.Object.Instantiate(prefab);
            newObject.transform.parent = root.transform;
            newObject.transform.ResetLocal();

            return newObject;
        }


        /// <summary>
        /// Instantiates a prefab, attaches it to the given root, and
        /// sets the local position and rotation.
        /// </summary>
        /// <param name="prefab">The prefab.</param>
        /// <param name="root">The root.</param>
        /// <param name="localPosition">The local position.</param>
        /// <param name="localRotation">The local rotation.</param>
        /// <returns>GameObject.</returns>
        public static GameObject Instantiate(GameObject prefab, GameObject root, Vector3 localPosition, Quaternion localRotation)
        {
            var newObj = (GameObject)UnityEngine.Object.Instantiate(prefab);

            newObj.transform.parent = newObj.transform;
            newObj.transform.localPosition = localPosition;
            newObj.transform.localRotation = localRotation;
            newObj.transform.ResetScale();

            return newObj;
        }


        #endregion
    }
}
