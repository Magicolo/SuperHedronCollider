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
		float distance = Vector3.Distance(transform.position, new Vector3(Layer.target.x, transform.position.y, Layer.target.z));
		
		if (distance > Layer.navMeshAgent.stoppingDistance) {
			SwitchState<TroopBaseMove>();
		}
	}
}
