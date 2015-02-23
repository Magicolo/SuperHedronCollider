using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

public class PlayerInputSelect : State {
	
	[Disable] public Vector3 selectionStart;
	
	LineRenderer _lineRenderer;
	public LineRenderer lineRenderer { get { return _lineRenderer ? _lineRenderer : (_lineRenderer = GetComponent<LineRenderer>()); } }
	
	PlayerInput Layer {
		get { return ((PlayerInput)layer); }
	}
	
	public override void OnEnter() {
		Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 100));
		selectionStart = mouseWorldPosition;
		
		lineRenderer.SetPosition(0, selectionStart);
		lineRenderer.SetPosition(1, selectionStart);
		lineRenderer.SetPosition(2, selectionStart);
		lineRenderer.SetPosition(3, selectionStart);
		lineRenderer.SetPosition(4, selectionStart);
		lineRenderer.enabled = true;
	}
	
	public override void OnExit() {
		lineRenderer.enabled = false;
	}
	
	public override void OnUpdate() {
		if (!Input.GetMouseButton(Layer.mouseSelectButton)) {
			SwitchState<PlayerInputIdle>(0);
			return;
		}
		
		Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 100));
		lineRenderer.SetPosition(0, new Vector3(selectionStart.x, 10, selectionStart.z));
		lineRenderer.SetPosition(1, new Vector3(mouseWorldPosition.x, 10, selectionStart.z));
		lineRenderer.SetPosition(2, new Vector3(mouseWorldPosition.x, 10, mouseWorldPosition.z));
		lineRenderer.SetPosition(3, new Vector3(selectionStart.x, 10, mouseWorldPosition.z));
		lineRenderer.SetPosition(4, new Vector3(selectionStart.x, 10, selectionStart.z));
	}
}
