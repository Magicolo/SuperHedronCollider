using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

public class PlayerInputSelect : State {
	
	[Disable] public Vector3 selectionStart;
	[Disable] public bool draw;
	
	PlayerInput Layer {
		get { return ((PlayerInput)layer); }
	}
	
	public override void OnEnter() {
		Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 100));
		selectionStart = mouseWorldPosition;
		draw = true;
	}
	
	public override void OnExit() {
		draw = false;
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
		
		Vector3 startScreenPosition = Camera.main.WorldToScreenPoint(selectionStart);
		startScreenPosition.y = Screen.height - startScreenPosition.y;
		Vector3 mouseScreenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z);
		mouseScreenPosition.y = Screen.height - mouseScreenPosition.y;
		
		Rect rect = new Rect(Mathf.Min(startScreenPosition.x, mouseScreenPosition.x), Mathf.Min(startScreenPosition.y, mouseScreenPosition.y), Mathf.Abs(mouseScreenPosition.x - startScreenPosition.x), Mathf.Abs(mouseScreenPosition.y - startScreenPosition.y));
		GUI.Box(rect, "");
	}
}
