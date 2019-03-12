using UnityEngine;
using UnityEngine.Events;

namespace Lean.Pool
{
	/// <summary>This component will automatically reset a Rigidbody when it gets spawned/despawned.</summary>
	[HelpURL(LeanPool.HelpUrlPrefix + "LeanPoolable")]
	public class LeanPoolable : MonoBehaviour
	{
		/// <summary>Called when this poolable object is spawned.</summary>
		public UnityEvent OnSpawn;

		/// <summary>Called when this poolable object is despawned.</summary>
		public UnityEvent OnDespawn;
	}
}