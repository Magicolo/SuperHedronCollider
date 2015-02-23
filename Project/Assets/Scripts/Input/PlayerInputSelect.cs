using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

public class PlayerInputSelect : State {
	
	[Disable] public Vector3 selectionStart;
	
	PlayerInput Layer {
		get { return ((PlayerInput)layer); }
	}
	
	public override void OnEnter() {
		Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 100));
		selectionStart = mouseWorldPosition;
	}
	
	public override void OnExit() {
		
	}
	
	public override void OnUpdate() {
		if (!Input.GetMouseButton(Layer.mouseSelectButton)) {
			SwitchState<PlayerInputIdle>(0);
			return;
		}
		
		Vector3 startViewPosition = Camera.main.WorldToViewportPoint(selectionStart);
		Vector3 mouseViewPosition = Camera.main.ScreenToViewportPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z));
		Vector3[] points = new Vector3[4];
		points[0] = new Vector3(startViewPosition.x, startViewPosition.y, 0);
		points[1] = new Vector3(mouseViewPosition.x, startViewPosition.y, 0);
		points[2] = new Vector3(mouseViewPosition.x, mouseViewPosition.y, 0);
		points[3] = new Vector3(startViewPosition.x, mouseViewPosition.y, 0);
		SelectionBoxRenderer.points = points;
	}
}
