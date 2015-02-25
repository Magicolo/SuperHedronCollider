using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

public class TroopBaseMove : State {
	
	TroopBase Layer {
		get { return ((TroopBase)layer); }
	}
	
	public override void OnEnter() {
		
	}
	
	public override void OnExit() {
		
	}
	
	public override void OnUpdate() {
		float distance = Vector3.Distance(transform.position, new Vector3(Layer.Target.x, transform.position.y, Layer.Target.z));
		
		if (distance < Layer.navMeshAgent.stoppingDistance) {
			SwitchState(Layer.GetType().Name + "Idle");
			return;
		}
	}
}
