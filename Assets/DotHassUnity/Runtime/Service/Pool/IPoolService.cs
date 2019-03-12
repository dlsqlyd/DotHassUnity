using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DotHass.Unity
{
    public interface IPoolService
    {
        T Spawn<T>(T prefab, Vector3 position, Quaternion rotation, Transform parent) where T : Component;


         GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent);

        // This will despawn all pool clones
         void DespawnAll();
        // This allows you to despawn a clone via GameObject, with optional delay
         void Despawn(GameObject clone, float delay = 0.0f);
    }
}
