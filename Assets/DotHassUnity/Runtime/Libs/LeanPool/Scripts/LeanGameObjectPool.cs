using UnityEngine;
using System.Collections.Generic;

namespace Lean.Pool
{
	/// <summary>This component allows you to pool GameObjects, allowing for a very fast instantiate/destroy alternative.</summary>
	[HelpURL(LeanPool.HelpUrlPrefix + "LeanGameObjectPool")]
	public class LeanGameObjectPool : MonoBehaviour
	{
		[System.Serializable]
		public class Clone
		{
			public GameObject   GameObject;
			public Transform    Transform;
			public LeanPoolable Poolable;
		}

		[System.Serializable]
		public class Delay
		{
			public GameObject GameObject;
			public float      Life;
		}

		public enum NotificationType
		{
			None,
			SendMessage,
			BroadcastMessage,
			PoolableEvent
		}

		/// <summary>All activle and enabled pools in the scene.</summary>
		public static List<LeanGameObjectPool> Instances = new List<LeanGameObjectPool>();

		/// <summary>The prefab this pool controls.</summary>
		[Tooltip("The prefab this pool controls.")]
		public GameObject Prefab;

		/// <summary>Should this pool send messages to the clones when they're spawned/despawned?</summary>
		[Tooltip("Should this pool send messages to the clones when they're spawned/despawned?")]
		public NotificationType Notification = NotificationType.SendMessage;

		/// <summary>Should this pool preload some clones?</summary>
		[Tooltip("Should this pool preload some clones?")]
		public int Preload;

		/// <summary>Should this pool have a maximum amount of spawnable clones?</summary>
		[Tooltip("Should this pool have a maximum amount of spawnable clones?")]
		public int Capacity;

		/// <summary>If the pool reaches capacity, should new spawns force older ones to despawn?</summary>
		[Tooltip("If the pool reaches capacity, should new spawns force older ones to despawn?")]
		public bool Recycle;

		/// <summary>Should this pool be marked as DontDestroyOnLoad?</summary>
		[Tooltip("Should this pool be marked as DontDestroyOnLoad?")]
		public bool Persist;

		/// <summary>Should the spawned cloned have the clone index appended to their name?</summary>
		[Tooltip("Should the spawned cloned have the clone index appended to their name?")]
		public bool Stamp;

		/// <summary>Should detected issues be output to the console?</summary>
		[Tooltip("Should detected issues be output to the console?")]
		public bool Warnings = true;

		/// <summary>All the currently spawned prefab instances.</summary>
		[SerializeField]
		private List<Clone> spawnedClones = new List<Clone>();

		/// <summary>All the currently despawned prefab instances.</summary>
		[SerializeField]
		private List<Clone> despawnedClones = new List<Clone>();

		/// <summary>All the delayed destruction objects.</summary>
		[SerializeField]
		private List<Delay> delays = new List<Delay>();

		/// <summary>Find the pool responsible for handling the specified prefab.</summary>
		public static bool TryFindPoolByPrefab(GameObject prefab, ref LeanGameObjectPool foundPool)
		{
			for (var i = Instances.Count - 1; i >= 0; i--)
			{
				var pool = Instances[i];

				if (pool.Prefab == prefab)
				{
					foundPool = pool; return true;
				}
			}

			return false;
		}

		/// <summary>Find the pool responsible for handling the specified prefab clone.
		/// NOTE: This can be an expensive operation if you have many large pools.</summary>
		public static bool TryFindPoolByClone(GameObject gameObject, ref LeanGameObjectPool foundPool)
		{
			for (var i = Instances.Count - 1; i >= 0; i--)
			{
				var pool = Instances[i];

				for (var j = pool.spawnedClones.Count - 1; j >= 0; j--)
				{
					var clone = pool.spawnedClones[j];

					if (clone.GameObject == gameObject)
					{
						foundPool = pool; return true;
					}
				}
			}

			return false;
		}

		/// <summary>Returns the amount of spawned clones.</summary>
		public int Spawned
		{
			get
			{
				return spawnedClones.Count;
			}
		}

		/// <summary>Returns the amount of despawned clones.</summary>
		public int Despawned
		{
			get
			{
				return despawnedClones.Count;
			}
		}

		/// <summary>Returns the total amount of spawned and despawned clones.</summary>
		public int Total
		{
			get
			{
				return Spawned + Despawned;
			}
		}

		/// <summary>This will either spawn a previously despanwed/preloaded clone, recycle one, create a new one, or return null.</summary>
		public GameObject Spawn(Vector3 position, Quaternion rotation, Transform parent = null)
		{
			if (Prefab != null)
			{
				// Spawn a previously despanwed/preloaded clone?
				// Loop through all despawnedClones until one is found
				while (despawnedClones.Count > 0)
				{
					var index = despawnedClones.Count - 1;
					var clone = despawnedClones[index];
						
					despawnedClones.RemoveAt(index);

					if (clone.GameObject != null)
					{
						SpawnClone(clone, position, rotation, parent);

						return clone.GameObject;
					}

					if (Warnings == true) Debug.LogWarning("This pool contained a null despawned clone, did you accidentally destroy it?", this);
				}

				// Make a new clone?
				if (Capacity <= 0 || Total < Capacity)
				{
					var clone = CreateClone(position, rotation, parent);

					// Add clone to spawned list
					spawnedClones.Add(clone);

					// Messages?
					InvokeOnSpawn(clone);

					return clone.GameObject;
				}

				// Recycle?
				if (Recycle == true)
				{
					// Loop through all spawnedClones from the front (oldest) until one is found
					while (spawnedClones.Count > 0)
					{
						var clone = spawnedClones[0];

						spawnedClones.RemoveAt(0);

						if (clone != null)
						{
							InvokeOnDespawn(clone);

							clone.GameObject.SetActive(false);

							SpawnClone(clone, position, rotation, parent);

							return clone.GameObject;
						}

						if (Warnings == true) Debug.LogWarning("This pool contained a null spawned clone, did you accidentally destroy it?", this);
					}
				}
			}
			else
			{
				if (Warnings == true) Debug.LogWarning("You're attempting to spawn from a pool with a null prefab", this);
			}

			return null;
		}

		/// <summary>This allows you to access the spawned clone at the specified index.</summary>
		public GameObject GetSpawned(int index)
		{
			return index >= 0 && index < spawnedClones.Count ? spawnedClones[index].GameObject : null;
		}

		/// <summary>This allows you to access the despawned clone at the specified index.</summary>
		public GameObject GetDespawned(int index)
		{
			return index >= 0 && index < despawnedClones.Count ? despawnedClones[index].GameObject : null;
		}

		[ContextMenu("Despawn All")]
		public void DespawnAll()
		{
			// Quickly despawn all clones
			for (var i = spawnedClones.Count - 1; i >= 0; i--)
			{
				DespawnNow(spawnedClones[i].GameObject);
			}

			spawnedClones.Clear();

			// Clear all delays
			for (var i = delays.Count - 1; i >= 0; i--)
			{
				LeanClassPool<Delay>.Despawn(delays[i]);
			}

			delays.Clear();
		}

		/// <summary>This will either instantly despawn the specified gameObject, or delay despawn it after t seconds.</summary>
		public void Despawn(GameObject cloneGameObject, float t = 0.0f)
		{
			if (cloneGameObject != null)
			{
				// Delay the despawn?
				if (t > 0.0f)
				{
					DespawnWithDelay(cloneGameObject, t);
				}
				// Despawn now?
				else
				{
					DespawnNow(cloneGameObject);

					// If this clone was marked for delayed despawn, remove it
					for (var i = delays.Count - 1; i >= 0; i--)
					{
						var delay = delays[i];

						if (delay.GameObject == cloneGameObject)
						{
							break;
						}
					}
				}
			}
			else
			{
				if (Warnings == true) Debug.LogWarning("You're attempting to despawn a null gameObject", this);
			}
		}

		/// <summary>This method will create an additional prefab clone and add it to the despawned list.</summary>
		[ContextMenu("Preload One More")]
		public void PreloadOneMore()
		{
			if (Prefab != null)
			{
				// Create clone
				var clone = CreateClone(Vector3.zero, Quaternion.identity, null);

				// Add clone to despawned list
				despawnedClones.Add(clone);

				// Deactivate it
				clone.GameObject.SetActive(false);

				// Move it under this GO
				clone.Transform.SetParent(transform, false);

				if (Warnings == true && Capacity > 0 && Total > Capacity) Debug.LogWarning("You've preloaded more than the pool capacity, please verify you're preloading the intended amount", this);
			}
			else
			{
				if (Warnings == true) Debug.LogWarning("Attempting to preload a null prefab", this);
			}
		}

		/// <summary>This will preload the pool based on the Preload setting.</summary>
		[ContextMenu("Preload All")]
		public void PreloadAll()
		{
			if (Preload > 0)
			{
				if (Prefab != null)
				{
					for (var i = Total; i < Preload; i++)
					{
						PreloadOneMore();
					}
				}
				else if (Warnings == true)
				{
					if (Warnings == true) Debug.LogWarning("Attempting to preload a null prefab", this);
				}
			}
		}

		protected virtual void Awake()
		{
			PreloadAll();

			if (Persist == true)
			{
				DontDestroyOnLoad(this);
			}
		}

		protected virtual void OnEnable()
		{
			Instances.Add(this);
		}

		protected virtual void OnDisable()
		{
			Instances.Remove(this);
		}

		protected virtual void Update()
		{
			// Decay the life of all delayed destruction calls
			for (var i = delays.Count - 1; i >= 0; i--)
			{
				var delay = delays[i];

				delay.Life -= Time.deltaTime;

				// Skip to next one?
				if (delay.Life > 0.0f)
				{
					continue;
				}

				// Remove and pool delay
				delays.RemoveAt(i); LeanClassPool<Delay>.Despawn(delay);

				// Finally despawn it after delay
				if (delay.GameObject != null)
				{
					Despawn(delay.GameObject);
				}
				else
				{
					if (Warnings == true) Debug.LogWarning("Attempting to update the delayed destruction of a prefab clone that no longer exists, did you accidentally delete it?", this);
				}
			}
		}

		private void DespawnWithDelay(GameObject cloneGameObject, float t)
		{
			// If this object is already marked for delayed despawn, update the time and return
			for (var i = delays.Count - 1; i >= 0; i--)
			{
				var delay = delays[i];

				if (delay.GameObject == cloneGameObject)
				{
					if (t < delay.Life)
					{
						delay.Life = t;
					}

					return;
				}
			}

			// Create delay
			var newDelay = LeanClassPool<Delay>.Spawn() ?? new Delay();

			newDelay.GameObject = cloneGameObject;
			newDelay.Life       = t;

			delays.Add(newDelay);
		}

		private void DespawnNow(GameObject cloneGameObject)
		{
			// Find the clone associated with this gameObject
			for (var i = spawnedClones.Count - 1; i >= 0; i--)
			{
				var clone = spawnedClones[i];

				if (clone.GameObject == cloneGameObject)
				{
					DespawnNow(clone);

					// Remove clone from spawned list
					spawnedClones.RemoveAt(i);

					return;
				}
			}

			if (Warnings == true) Debug.LogWarning("You're attempting to despawn a gameObject that wasn't spawned from this pool, make sure your Spawn and Despawn calls match", cloneGameObject);
		}

		private void DespawnNow(Clone clone)
		{
			// Add clone to despawned list
			despawnedClones.Add(clone);

			// Messages?
			InvokeOnDespawn(clone);

			// Deactivate it
			clone.GameObject.SetActive(false);

			// Move it under this GO
			clone.Transform.SetParent(transform, false);
		}

		private Clone CreateClone(Vector3 position, Quaternion rotation, Transform parent)
		{
			var clone = new Clone();

			clone.GameObject = (GameObject)Instantiate(Prefab, position, rotation);
			clone.Transform  = clone.GameObject.transform;

			if (Stamp == true)
			{
				clone.GameObject.name = Prefab.name + " " + Total;
			}
			else
			{
				clone.GameObject.name = Prefab.name;
			}

			clone.Transform.SetParent(parent, false);

			return clone;
		}

		private void SpawnClone(Clone clone, Vector3 position, Quaternion rotation, Transform parent)
		{
			// Add clone to spawned list
			spawnedClones.Add(clone);

			// Update transform of clone
			var cloneTransform = clone.Transform;

			cloneTransform.localPosition = position;
			cloneTransform.localRotation = rotation;

			cloneTransform.SetParent(parent, false);

			// Activate clone
			clone.GameObject.SetActive(true);

			// Notifications
			InvokeOnSpawn(clone);
		}

		private static LeanPoolable GetOrCachePoolable(Clone clone)
		{
			var poolable = clone.Poolable;

			if (clone.Poolable == null)
			{
				poolable = clone.Poolable = clone.GameObject.GetComponent<LeanPoolable>();
			}

			return poolable;
		}

		private void InvokeOnSpawn(Clone clone)
		{
			switch (Notification)
			{
				case NotificationType.SendMessage: clone.GameObject.SendMessage("OnSpawn", SendMessageOptions.DontRequireReceiver); break;
				case NotificationType.BroadcastMessage: clone.GameObject.BroadcastMessage("OnSpawn", SendMessageOptions.DontRequireReceiver); break;
				case NotificationType.PoolableEvent: var poolable = GetOrCachePoolable(clone); if (poolable != null && poolable.OnSpawn != null) poolable.OnSpawn.Invoke(); break;
			}
		}

		private void InvokeOnDespawn(Clone clone)
		{
			switch (Notification)
			{
				case NotificationType.SendMessage: clone.GameObject.SendMessage("OnDespawn", SendMessageOptions.DontRequireReceiver); break;
				case NotificationType.BroadcastMessage: clone.GameObject.BroadcastMessage("OnDespawn", SendMessageOptions.DontRequireReceiver); break;
				case NotificationType.PoolableEvent: var poolable = GetOrCachePoolable(clone); if (poolable != null && poolable.OnDespawn != null) poolable.OnDespawn.Invoke(); break;
			}
		}
	}
}