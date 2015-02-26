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
			
		if (Layer.selectedTroops.Count > 0 && Physics.Raycast(mouseRay, out mouseRayInfo) && mouseRayInfo.point.y <= 3) {
			int groupId;
			int[] groupIds = TroopManager.ContainingGroupZones(NetworkController.CurrentPlayerId, mouseRayInfo.point);
			
			if (groupIds.Length > 0) {
				groupId = groupIds[0];
				TroopManager.SwitchTroopsToGroup(NetworkController.CurrentPlayerId, groupId, Layer.selectedTroops.ToArray());
			}
			else {
				groupId = TroopManager.CreateGroup(NetworkController.CurrentPlayerId, Layer.selectedTroops.ToArray());
			}
			
			TroopManager.MoveGroup(NetworkController.CurrentPlayerId, groupId, mouseRayInfo.point);
		
			if (NetworkController.instance.isConnected) {
				foreach (TroopBase troop in Layer.selectedTroops) {
					NetworkController.instance.clientController.sendUnitTarget(troop.id, troop.Target);
				}
			}
		}
	}
}
