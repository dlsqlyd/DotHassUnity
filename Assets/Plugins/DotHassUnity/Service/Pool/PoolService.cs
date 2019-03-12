
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Lean.Pool;

namespace HFramework
{
    public class PoolService: IPoolService
    {
        public  T Spawn<T>(T prefab, Vector3 position, Quaternion rotation, Transform parent) where T : Component
        {
            return LeanPool.Spawn<T>(prefab, position, rotation, parent);
        }


        public  GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent)
        {
            return LeanPool.Spawn(prefab,position,rotation,parent); 
        }

        // This will despawn all pool clones
        public  void DespawnAll()
        {
            LeanPool.DespawnAll();
        }

        // This allows you to despawn a clone via GameObject, with optional delay
        public void Despawn(GameObject clone, float delay = 0.0f)
        {
            LeanPool.Despawn(clone, delay);
        }
    }


    public static class PoolServiceExtensions
    {
        public static GameObject Spawn(this IPoolService pool, GameObject prefab, Vector3 position, Transform parent) 
        {
            return pool.Spawn(prefab, position, Quaternion.identity, parent);
        }

        public static T Spawn<T>(this IPoolService pool, T prefab, Vector3 position, Transform parent) where T : Component
        {
            return pool.Spawn(prefab,  position, Quaternion.identity, parent);
        }

        public static T Spawn<T>(this IPoolService pool , T prefab) where T : Component
        {
            return pool.Spawn(prefab, Vector3.zero, Quaternion.identity, null);
        }

        public static T Spawn<T>(this IPoolService pool,T prefab, Vector3 position, Quaternion rotation) where T : Component
        {
            return pool.Spawn(prefab, position, rotation, null);
        }

        // These methods allows you to spawn prefabs via GameObject with varying levels of transform data
        public static  GameObject Spawn(this IPoolService pool, GameObject prefab)
        {
            return pool.Spawn(prefab, Vector3.zero, Quaternion.identity, null);
        }

        public static GameObject Spawn(this IPoolService pool, GameObject prefab, Transform parent)
        {
            return pool.Spawn(prefab, Vector3.zero, Quaternion.identity, parent);
        }

        public static GameObject Spawn(this IPoolService pool, GameObject prefab, Vector3 position, Quaternion rotation)
        {
            return pool.Spawn(prefab, position, rotation, null);
        }

        // This allows you to despawn a clone via Component, with optional delay
        public static void Despawn(this IPoolService pool, Component clone, float delay = 0.0f)
        {
            if (clone != null) pool.Despawn(clone.gameObject, delay);
        }
    }
}
