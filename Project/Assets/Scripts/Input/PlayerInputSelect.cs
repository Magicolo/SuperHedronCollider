using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

public class PlayerInputSelect : State {
	
	public int boxMargin = 10;
	public bool cornersOnly;
	public Color boxColor = Color.green;
	public float minBoxSize = 10;
	[Disable] public Vector3 selectionStart;
	[Disable] public Vector3 selectionEnd;
	[Disable] public Rect selectionRect;
	[Disable] public bool draw;
	
	Texture2D boxTexture;
	
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
	
	public override void OnAwake() {
		boxTexture = new Texture2D(128, 128, TextureFormat.RGBA32, true);
		boxTexture.filterMode = FilterMode.Point;
		
		for (int x = 0; x < boxTexture.width; x++) {
			for (int y = 0; y < boxTexture.height; y++) {
				if (cornersOnly && (((x == 0 || x == boxTexture.width - 1) && (y < boxMargin || y >= boxTexture.height - boxMargin)) || ((y == 0 || y == boxTexture.height - 1) && (x < boxMargin || x >= boxTexture.width - boxMargin)))) {
					boxTexture.SetPixel(x, y, boxColor);
				}
				else if (x < boxMargin || x >= boxTexture.width - boxMargin || y < boxMargin || y >= boxTexture.height - boxMargin) {
					boxTexture.SetPixel(x, y, boxColor);
				}
				else {
					boxTexture.SetPixel(x, y, boxColor / 2);
				}
			}
		}
		
		boxTexture.Apply();
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
		
		if (GUI.skin.box.normal.background != boxTexture) {
			GUI.skin.box.normal.background = boxTexture;
			GUI.skin.box.border = new RectOffset(boxMargin, boxMargin, boxMargin, boxMargin);
		}
		
		selectionEnd = new Vector3(Input.mousePosition.x, Screen.height - Input.mousePosition.y, Camera.main.transform.position.z);
		selectionRect = new Rect(Mathf.Min(selectionStart.x, selectionEnd.x), Mathf.Min(selectionStart.y, selectionEnd.y), Mathf.Abs(selectionEnd.x - selectionStart.x), Mathf.Abs(selectionEnd.y - selectionStart.y));
		
		if (selectionRect.size.magnitude > minBoxSize) {
			GUI.Box(selectionRect, "");
		}
	}

	void Select() {
		DeselectAll();
		
		TroopBase[] troops = TroopManager.GetTroops(NetworkController.CurrentPlayerId);
		
		if (selectionRect.size.magnitude <= minBoxSize) {
			Ray selectionRay = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit selectionRayInfo;
				
			if (Physics.Raycast(selectionRay, out selectionRayInfo)) {
				TroopBase troop = selectionRayInfo.collider.GetComponent<TroopBase>();
				
				if (troop != null && troop.playerId == NetworkController.CurrentPlayerId) {
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
