using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

public class PlayerInputCamera : State {
	
	[SerializeField, PropertyField]
	float dragMargin = 50;
	public float DragMargin {
		get {
			return dragMargin;
		}
		set {
			dragMargin = value;
			noDragZone = new Rect(dragMargin, dragMargin, Screen.width - dragMargin * 2, Screen.height - dragMargin * 2);
		}
	}
	
	public float moveSpeed = 5;
	public float moveSmooth = 5;
	public float zoomSpeed = 5;
	public float zoomSmooth = 5;
	public float minZoom = 5;
	public float maxZoom = 120;
	
	[Disable] public float smoothScrollDelta;
	[Disable] public Vector3 dragStart;
	[Disable] public Vector3 dragEnd;
	[Disable] public Vector3 dragDelta;
	[Disable] public Rect noDragZone;
	
	PlayerInput Layer {
		get { return ((PlayerInput)layer); }
	}
	
	public override void OnAwake() {
		noDragZone = new Rect(dragMargin, dragMargin, Screen.width - dragMargin * 2, Screen.height - dragMargin * 2);
	}
	
	public override void OnEnter() {
		
	}
	
	public override void OnExit() {
		
	}
	
	public override void OnUpdate() {
		Move();
		Zoom();
	}

	void Move() {
		if (Input.GetMouseButtonDown(Layer.mouseMoveButton)) {
			dragStart = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.y);
		}
		else if (Input.GetMouseButton(Layer.mouseMoveButton)) {
			dragEnd = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.y);
			dragDelta = Vector3.Lerp(dragDelta, Camera.main.ScreenToWorldPoint(dragStart) - Camera.main.ScreenToWorldPoint(dragEnd), Time.deltaTime * moveSmooth);
			dragStart = dragEnd;
		}
		else if (!new Rect(dragMargin, dragMargin, Screen.width - dragMargin * 2, Screen.height - dragMargin * 2).Contains(Input.mousePosition)) {
			Vector3 direction = (Input.mousePosition - new Vector3(Screen.width / 2, Screen.height / 2, Input.mousePosition.z)) / 1000;
			dragDelta = Vector3.Lerp(dragDelta, new Vector3(direction.x, 0, direction.y), Time.deltaTime * moveSmooth);
		}
		else {
			dragDelta = Vector3.Lerp(dragDelta, Vector3.zero, Time.deltaTime * moveSmooth);
		}
		
		Camera.main.transform.Translate(dragDelta * moveSpeed, "XZ");
	}
	
	void Zoom() {
		smoothScrollDelta = Mathf.Lerp(smoothScrollDelta, Input.mouseScrollDelta.y, Time.deltaTime * zoomSmooth);
		Camera.main.transform.SetPosition(Mathf.Clamp(Camera.main.transform.position.y - smoothScrollDelta * zoomSpeed, minZoom, maxZoom), "Y");
	}
}
