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
		
		foreach (ISelectable selected in Layer.selectedTroops) {
			if (selected as TroopBase != null) {
				TroopBase troop = selected as TroopBase;
				Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit mouseRayInfo;
				
				if (Physics.Raycast(mouseRay, out mouseRayInfo)) {
					troop.Target = mouseRayInfo.point;
				}
			}
		}
	}
	
}
