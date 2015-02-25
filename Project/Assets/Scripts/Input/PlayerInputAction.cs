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
			
		if (Layer.selectedTroops.Count > 0 && Physics.Raycast(mouseRay, out mouseRayInfo) && mouseRayInfo.point.y <= 0.5F) {
			int troopCounter = 0;
			float troopRadius = Layer.selectedTroops[0].navMeshAgent.radius;
			int segmentLength = 1;
			Vector3 lastPosition = mouseRayInfo.point;
			Vector3 currentDirection = Vector3.forward;
			
			while (troopCounter < Layer.selectedTroops.Count) {
				for (int i = 0; i < segmentLength / 2;) {
					TroopBase selectedTroop = Layer.selectedTroops[troopCounter];
					
					if (selectedTroop.gameObject.activeInHierarchy && selectedTroop.Selected) {
						selectedTroop.Target = lastPosition;
						lastPosition += currentDirection * selectedTroop.radius;
						i++;
					}
					
					troopCounter += 1;
					
					if (troopCounter >= Layer.selectedTroops.Count) {
						break;
					}
				}
				
				currentDirection = currentDirection.Rotate(90, Vector3.up);
				segmentLength += 1;
			}
//			for (int i = 0; i < Layer.selectedTroops.Count; i++) {
//				TroopBase selectedTroop = Layer.selectedTroops[i];
//				
//				if (selectedTroop.gameObject.activeInHierarchy && selectedTroop.Selected) {
//					selectedTroop.Target = mouseRayInfo.point;
//				}
//			}
		}
	}
	
}
