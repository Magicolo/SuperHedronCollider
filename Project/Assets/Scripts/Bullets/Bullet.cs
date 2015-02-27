using UnityEngine;
using System.Collections;
using Magicolo;

public class Bullet : MonoBehaviour {
	
	public float rotateSpeed = 5;
	
	[Disable] public float lifeCounter;
	[Disable] public float damage;
	[Disable] public TroopBase source;
	[Disable] public TroopBase target;
	[Disable] public int playerId;
	[Disable] public int id;
	
	void Update() {
		lifeCounter -= Time.deltaTime;
		Rotate();
		
		if (source.playerId == NetworkController.CurrentPlayerId && lifeCounter <= 0) {
			Kill();
			return;
		}
		
		if (source != null && target != null && source.gameObject.activeInHierarchy && target.gameObject.activeInHierarchy) {
			transform.LookAt(target.transform);
		}
		
		transform.Translate(transform.forward * source.bulletSpeed * Time.deltaTime, "XZ");
	}
	
	void OnTriggerEnter(Collider collision) {
		TroopBase troop = collision.GetComponent<TroopBase>();
			
		if (troop != null && playerId != troop.playerId && playerId == NetworkController.CurrentPlayerId) {
			dammageTroop(troop, damage);
		}
		
		Kill();
	}

	void Rotate() {
		transform.Rotate(rotateSpeed, "Z");
	}
	
	void dammageTroop(TroopBase troop, float damage) {
		if (!NetworkController.instance.isConnected) {
			troop.Damage(source.damage);
		}
		else {
			NetworkController.instance.clientController.sendUnitDamage(troop.playerId, troop.id, damage);
		}
	}

	public void Move(Vector3 position) {
		transform.position = position;
	}
	
	public void Kill() {
		if (!NetworkController.instance.isConnected) {
			BulletManager.Despawn(this);
		}
		else {
			// FIXME ceci ne ferai AUCUN bug si le troop est deactivé et reacivé pour un autre troop pendant que la balle se promene
			NetworkController.instance.clientController.killBullet(source.playerId, id);
		}
	}
}
