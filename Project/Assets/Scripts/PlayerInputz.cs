using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Magicolo;

public class PlayerInputz : MonoBehaviourExtended {

	public int mouseSelectButton = 0;
	public int mouseMoveButton = 1;
	[Disable] public Vector3 selectionStart;
	[Disable] public bool selectionStarted;
	[Disable] public List<TroopBase> selectedTroops;
	
	void Update() {
		SelectTroops();
		MoveTroops();
	}

	void SelectTroops() {
		if (selectionStarted) {
			if (Input.GetMouseButtonUp(mouseSelectButton)) {
				selectionStarted = false;
			}
		}
		else if (Input.GetMouseButtonDown(mouseSelectButton)) {
			selectionStart = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, 0, Input.mousePosition.z));
			selectionStarted = true;
		}
	}
	
	void MoveTroops() {
		if (Input.GetMouseButton(mouseMoveButton)) {
			foreach (TroopBase troop in selectedTroops) {
				Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit mouseRayInfo;
				
				if (Physics.Raycast(mouseRay, out mouseRayInfo)) {
					troop.Target = mouseRayInfo.point;
				}
			}
		}
	}
}
