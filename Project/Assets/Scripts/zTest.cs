using UnityEngine;
using System.Collections;
using Magicolo;

public class zTest : MonoBehaviour {

	public float lifeTime = 2;
	
	[Button("Spawn", "Spawn", NoPrefixLabel = true)] public bool spawn;
	void Spawn() {
		GameObject spawned = hObjectPool.Instance.Spawn(References.TroopPrefab, transform.position, Quaternion.identity);
		StartCoroutine(DespawnAfterDelay(spawned));
	}
	
	void Update() {
		transform.OscillatePosition(2, 10, "X");
	}
	
	IEnumerator DespawnAfterDelay(GameObject toDespawn) {
		float counter = lifeTime;
		
		while (counter > 0) {
			counter -= Time.deltaTime;
			yield return new WaitForSeconds(0);
		}
		
		hObjectPool.Instance.Despawn(toDespawn);
	}
}
