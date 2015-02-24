using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

public class TroopBaseAttack : State {
	
	[Disable] public float attackCounter;
	
	TroopBase Layer {
		get { return ((TroopBase)layer); }
	}
	
	public override void OnAwake() {
		base.OnAwake();
		attackCounter = Layer.attackSpeed;
	}
	
	public override void OnEnter() {
		
	}
	
	public override void OnExit() {
		
	}
	
	public override void OnUpdate() {
		if (!Layer.CheckForEnemies()) {
			SwitchState(Layer.GetType().Name + "Idle");
			return;
		}
		
		transform.LookAt(Layer.closestInRangeEnemy.transform);
		
		Attack();
	}
	
	void Attack() {
		attackCounter -= Time.deltaTime;
		if (attackCounter <= 0) {
			attackCounter = 1F / Layer.attackSpeed;
			Shoot();
		}
	}
	
	void Shoot() {
		
	}
}
