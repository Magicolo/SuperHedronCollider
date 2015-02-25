using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

public class TroopBaseDead : State {
	
	TroopBase Layer {
		get { return ((TroopBase)layer); }
	}
	
	public override void OnEnter() {
		Layer.Despawn();
	}
	
	public override void OnExit() {
		
	}
	
	public override void OnUpdate() {
		
	}
}
