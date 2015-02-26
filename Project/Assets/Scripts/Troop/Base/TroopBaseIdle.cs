using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

public class TroopBaseIdle : State {
	
	TroopBase Layer {
		get { return ((TroopBase)layer); }
	}
	
	public override void OnEnter() {
		
	}
	
	public override void OnExit() {
		
	}
	
	public override void OnUpdate() {
		float distance = Vector3.Distance(transform.position, new Vector3(Layer.Target.x, transform.position.y, Layer.Target.z));
		
		if (distance > Layer.navMeshAgent.stoppingDistance) {
			SwitchState(Layer.GetType().Name + "Move");
			return;
		}
		
		if (Layer.CheckForEnemies()) {
//			 && Physics.Raycast(transform.position, (Layer.closestInRangeEnemy.transform.position - transform.position).normalized, Mathf.Infinity, new LayerMask().AddToMask(10, 11)
			Debug.DrawRay(transform.position, Layer.closestInRangeEnemy.transform.position - transform.position, Color.yellow, 5);
			
			SwitchState(Layer.GetType().Name + "Attack");
			return;
		}
	}
}
