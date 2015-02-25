using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

public class PlayerInputAction : State {
	
	PlayerInput Layer {
		get { return ((PlayerInput)layer); }
	}
	
	public override void OnEnter() {
		
	}
	
	public override void OnExit() {
		
	}
	
	public override void OnUpdate() {
		if (!Input.GetMouseButton(Layer.mouseActionButton)) {
			SwitchState<PlayerInputIdle>(1);
			return;
		}
		
		foreach (TroopBase selectedTroop in Layer.selectedTroops) {
			Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit mouseRayInfo;
				
			if (selectedTroop.gameObject.activeSelf && selectedTroop.Selected && Physics.Raycast(mouseRay, out mouseRayInfo)) {
				selectedTroop.Target = mouseRayInfo.point;
			}
		}
	}
	
}
