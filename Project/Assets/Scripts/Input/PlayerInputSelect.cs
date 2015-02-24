using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

public class PlayerInputSelect : State {
	
	public float minBoxSize = 1;
	[Disable] public Vector3 selectionStart;
	[Disable] public Vector3 selectionEnd;
	[Disable] public Rect selectionRect;
	[Disable] public bool draw;
	
	PlayerInput Layer {
		get { return ((PlayerInput)layer); }
	}
	
	public override void OnEnter() {
		selectionStart = new Vector3(Input.mousePosition.x, Screen.height - Input.mousePosition.y, Camera.main.transform.position.z);
		draw = true;
	}
	
	public override void OnExit() {
		Select();
		draw = false;
		selectionStart = Vector3.zero;
		selectionEnd = Vector3.zero;
		selectionRect = new Rect();
	}
	
	public override void OnUpdate() {
		if (!Input.GetMouseButton(Layer.mouseSelectButton)) {
			SwitchState<PlayerInputIdle>(0);
			return;
		}
	}
	
	public void OnGUI() {
		if (!draw) {
			return;
		}
		
		selectionEnd = new Vector3(Input.mousePosition.x, Screen.height - Input.mousePosition.y, Camera.main.transform.position.z);
		selectionRect = new Rect(Mathf.Min(selectionStart.x, selectionEnd.x), Mathf.Min(selectionStart.y, selectionEnd.y), Mathf.Abs(selectionEnd.x - selectionStart.x), Mathf.Abs(selectionEnd.y - selectionStart.y));
		
		if (selectionRect.size.magnitude > minBoxSize) {
			GUI.Box(selectionRect, "");
		}
	}

	void Select() {
		DeselectAll();
		
		TroopBase[] troops = FindObjectsOfType<TroopBase>();
		
		if (selectionRect.size.magnitude <= minBoxSize) {
			Ray selectionRay = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit selectionRayInfo;
				
			if (Physics.Raycast(selectionRay, out selectionRayInfo)) {
				TroopBase troop = selectionRayInfo.collider.GetComponent<TroopBase>();
				
				if (troop != null) {
					troop.Selected = true;
					Layer.selectedTroops.Add(troop);
				}
			}
		}
		else {
			foreach (TroopBase troop in troops) {
				Vector3 troopScreenPosition = Camera.main.WorldToScreenPoint(troop.transform.position);
				troopScreenPosition.y = Screen.height - troopScreenPosition.y;
				
				if (selectionRect.Contains(troopScreenPosition, true)) {
					troop.Selected = true;
					Layer.selectedTroops.Add(troop);
				}
			}
		}
	}
	
	void DeselectAll() {
		foreach (TroopBase troop in Layer.selectedTroops) {
			troop.Selected = false;
		}
		
		Layer.selectedTroops.Clear();
	}
}
