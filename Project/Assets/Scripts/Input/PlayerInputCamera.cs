using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

public class PlayerInputCamera : State {
	
	bool focused;
	
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
	public float maxMoveX = 100;
	public float maxMoveY = 25;
	
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
		if (focused) {
			Move();
			Zoom();
		}
		
	}

	void OnApplicationFocus(bool focus) {
		focused = focus;
	}
	
	void Move() {
		Vector3 keyMove = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * (Input.GetKey(Layer.fastMoveKey1) || Input.GetKey(Layer.fastMoveKey2) ? 6 : 3);
		
		if (Input.GetMouseButtonDown(Layer.mouseMoveButton)) {
			dragStart = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.y);
		}
		else if (Input.GetMouseButton(Layer.mouseMoveButton)) {
			dragEnd = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.y);
			dragDelta = Vector3.Lerp(dragDelta, Camera.main.ScreenToWorldPoint(dragStart) - Camera.main.ScreenToWorldPoint(dragEnd), Time.deltaTime * moveSmooth);
			dragStart = dragEnd;
		}
		else if (Layer.mouseMarginDrag && !new Rect(dragMargin, dragMargin, Screen.width - dragMargin * 2, Screen.height - dragMargin * 2).Contains(Input.mousePosition)) {
			Vector3 direction = (Input.mousePosition - new Vector3(Screen.width / 2, Screen.height / 2, Input.mousePosition.z)) / 250;
			dragDelta = Vector3.Lerp(dragDelta, new Vector3(direction.x, 0, direction.y), Time.deltaTime * moveSmooth);
		}
		else {
			dragDelta = Vector3.Lerp(dragDelta, Vector3.zero, Time.deltaTime * moveSmooth);
		}
		
		Vector3 targetPosition = Camera.main.transform.position + (dragDelta + keyMove) * moveSpeed;
		float zoomModifier = Mathf.Min(maxZoom / Camera.main.transform.position.y, 3);
		
		Camera.main.transform.SetPosition(new Vector3(Mathf.Clamp(targetPosition.x, -maxMoveX * zoomModifier, maxMoveX * zoomModifier), targetPosition.y, Mathf.Clamp(targetPosition.z, -maxMoveY * zoomModifier, maxMoveY * zoomModifier)), "XZ");
	}
	
	void Zoom() {
		float keyZoom = (float)((Input.GetKey(Layer.zoomKey1) || Input.GetKey(Layer.zoomKey2) || Input.GetKey(Layer.zoomKey3)).GetHashCode() - (Input.GetKey(Layer.dezoomKey1) || Input.GetKey(Layer.dezoomKey2) || Input.GetKey(Layer.dezoomKey3)).GetHashCode()) / 5;
		smoothScrollDelta = Mathf.Lerp(smoothScrollDelta, Input.mouseScrollDelta.y + keyZoom, Time.deltaTime * zoomSmooth);
		
		Camera.main.transform.SetPosition(Mathf.Clamp(Camera.main.transform.position.y - (smoothScrollDelta + keyZoom) * zoomSpeed, minZoom, maxZoom), "Y");
	}
}
