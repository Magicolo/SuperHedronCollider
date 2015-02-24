using UnityEngine;
using System.Collections;
using Magicolo;

public class Bullet : MonoBehaviour {
	
	[Disable] public float lifeCounter;
	[Disable] public TroopBase source;
	[Disable] public TroopBase target;
	[Disable] public int id;
	
	void Update() {
		lifeCounter -= Time.deltaTime;
		
		if (source.playerId == NetworkController.CurrentPlayerId && (lifeCounter <= 0 || !source.gameObject.activeInHierarchy || !target.gameObject.activeInHierarchy)) {
			Kill();
		}
		
		transform.LookAt(target.transform);
		transform.Translate(transform.forward * source.bulletSpeed * Time.deltaTime, "XZ");
	}
	
	void OnTriggerEnter(Collider collision) {
		if (!source.gameObject.activeInHierarchy || !target.gameObject.activeInHierarchy || source.playerId != NetworkController.CurrentPlayerId) {
			return;
		}
		
		TroopBase troop = collision.GetComponent<TroopBase>();
		
		if (troop != null && troop.playerId != source.playerId) {
			troop.Damage(source.damage);
			Despawn();
		}
	}
	
	public void Despawn() {
		BulletManager.Despawn(this);
	}

	public void Move(Vector3 position) {
		transform.position = position;
	}
	
	public void Kill() {
		Despawn();
	}
}
