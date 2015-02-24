using UnityEngine;
using System.Collections;

public class Pooled : Hydrogen.Core.ObjectPoolItemBase {

	public void Despawn() {
		hObjectPool.Instance.Despawn(gameObject);
	}
	
	public override void DespawnSafely() {
		StartCoroutine(WaitForParticles());
	}

	public override bool IsInactive() {
		return ParentPool.HasRigidbody && gameObject.rigidbody.IsSleeping();
	}

	public override void OnDespawned() {
		if (ParentPool.HasRigidbody) {
			gameObject.rigidbody.velocity = Vector3.zero;
		}
		
		gameObject.SetActive(false);
		SendMessage("Despawned", SendMessageOptions.DontRequireReceiver);
	}

	public override void OnSpawned() {
		gameObject.SetActive(true);
		SendMessage("Spawned", SendMessageOptions.DontRequireReceiver);
	}
	
	IEnumerator WaitForParticles() {
		if (particleEmitter != null) {
			yield return null;
			yield return new WaitForEndOfFrame();

			while (particleEmitter.particleCount > 0) {
				yield return null;
			}
			particleEmitter.emit = false;
		}
		else if (particleSystem != null) {
			yield return new WaitForSeconds(particleSystem.startDelay + 0.25f);
			while (particleSystem.IsAlive(true)) {
				if (!particleSystem.gameObject.activeSelf) {
					particleSystem.Clear(true);
					yield break;
				}
				yield return null;
			}
		}

		gameObject.SetActive(false);
		hObjectPool.Instance.ObjectPools[PoolID].DespawnImmediate(gameObject);
	}
}
