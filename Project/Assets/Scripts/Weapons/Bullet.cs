﻿using UnityEngine;
using System.Collections;
using Magicolo;

public class Bullet : MonoBehaviour {
	
	public TroopBase source;
	public TroopBase target;
	
	void Update() {
		transform.TranslateTowards(target.transform.position, source.bulletSpeed, InterpolationModes.Linear);
	}
	
	void OnTriggerEnter(Collider collision) {
		if (source.playerId != NetworkController.CurrentPlayerId) {
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
}