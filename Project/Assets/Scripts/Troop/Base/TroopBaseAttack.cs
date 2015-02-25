using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

public class TroopBaseAttack : State {
	
	[Disable] public float attackCounter;
	[Disable] public Coroutine attackCoroutine;
	
	TroopBase Layer {
		get { return ((TroopBase)layer); }
	}
	
	public override void OnAwake() {
		base.OnAwake();
		attackCounter = Layer.attackSpeed;
	}
	
	public override void OnEnter() {
//		attackCoroutine = StartCoroutine(Attack());
		attackCoroutine = StartCoroutine("Attack");
	}
	
	public override void OnExit() {
		StopCoroutine(attackCoroutine);
	}
	
	public override void OnUpdate() {
		if (!Layer.CheckForEnemies()) {
			SwitchState(Layer.GetType().Name + "Idle");
			return;
		}
		
		transform.LookAt(Layer.closestInRangeEnemy.transform);
	}
	
	IEnumerator Attack() {
		while (true) {
			attackCounter -= Time.deltaTime;
			
			if (attackCounter <= 0) {
				int burstCounter = Layer.bulletBurst;
				
				while (burstCounter > 0) {
					yield return new WaitForSeconds(Layer.bulletBurst / (Layer.attackSpeed * 25));
					
					burstCounter -= 1;
					Shoot();
				}
			
				attackCounter = 1F / Layer.attackSpeed;
			}
			
			yield return new WaitForSeconds(0);
		}
	}
	
	void Shoot() {
		if(!NetworkController.instance.isConnected){
			BulletManager.Spawn(Random.Range(0, int.MaxValue), Layer, Layer.closestInRangeEnemy);
		}else{
			TroopBase other = Layer.closestInRangeEnemy;
			NetworkController.instance.clientController.spawnBullet(Layer.playerId,Layer.id, other.playerId, other.id);
		}
		
	}
}
