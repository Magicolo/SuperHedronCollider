using UnityEngine;
using System.Collections;
using Magicolo;

public class Bullet : MonoBehaviour {
	
	public float rotateSpeed = 5;
	
	[Disable] public float lifeCounter;
	[Disable] public TroopBase source;
	[Disable] public TroopBase target;
	[Disable] public int id;
	
	void Update() {
		lifeCounter -= Time.deltaTime;
		Rotate();
		
		if (source == null || target == null) {
			Debug.Log("a bien kill dans update" );
			Kill();
		}
		else if (source.playerId == NetworkController.CurrentPlayerId && (lifeCounter <= 0 || !source.gameObject.activeInHierarchy || !target.gameObject.activeInHierarchy)) {
			Debug.Log("a bien kill plus dans update" );
			Kill();
		}
		else {
			transform.LookAt(target.transform);
			transform.Translate(transform.forward * source.bulletSpeed * Time.deltaTime, "XZ");
		}
	}
	
	void OnTriggerEnter(Collider collision) {
		if (source == null || target == null) {
			Debug.Log("a bien kill");
			Kill();
		}
		else if (!source.gameObject.activeInHierarchy || !target.gameObject.activeInHierarchy || source.playerId != NetworkController.CurrentPlayerId) {
			Debug.Log("Skip");
			return;
		}
		else {
			
			Debug.Log("On touche");
			TroopBase troop = collision.GetComponent<TroopBase>();
			
			if (troop != null && troop.playerId != source.playerId) {
				dammageTroop(troop, source.damage);
				
				Kill();
			}
		}
		
		
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
