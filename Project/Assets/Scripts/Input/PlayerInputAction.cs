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
		
		Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit mouseRayInfo;
			
		if (Layer.selectedTroops.Count > 0 && Physics.Raycast(mouseRay, out mouseRayInfo)) {
			float troopRadius = Layer.selectedTroops[0].navMeshAgent.radius;
			
			for (int i = 0; i < Layer.selectedTroops.Count; i++) {
				TroopBase selectedTroop = Layer.selectedTroops[i];
				
				if (selectedTroop.gameObject.activeInHierarchy && selectedTroop.Selected) {
					selectedTroop.Target = mouseRayInfo.point;
				}
			}
		}
	}
	
}
