using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

public class PlayerInputIdle : State {
	
	PlayerInput Layer {
		get { return ((PlayerInput)layer); }
	}
	
	public override void OnEnter() {
		
	}
	
	public override void OnExit() {
		
	}
	
	public override void OnUpdate() {
		if (Input.GetMouseButtonDown(Layer.mouseSelectButton) && GetActiveState(0) == this) {
			SwitchState<PlayerInputSelect>(0);
			return;
		}
		
		if (Input.GetMouseButtonDown(Layer.mouseActionButton) && GetActiveState(1) == this) {
			SwitchState<PlayerInputAction>(1);
			return;
		}
	}
	
}
