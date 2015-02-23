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
		selectionStart = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, 0, Input.mousePosition.z));
	}
	
	public override void OnExit() {
		
	}
	
	public override void OnUpdate() {
		if (!Input.GetMouseButton(Layer.mouseSelectButton)) {
			SwitchState<PlayerInputIdle>(0);
			return;
		}
	}
	
	void OnPostRender() {
//		GL.Begin(GL.LINES);
//		GL.Color(Color(1, 1, 1, 0.5));
//		GL.Vertex3(0, 0, 0);
//		GL.Vertex3(1, 0, 0);
//		GL.Vertex3(0, 1, 0);
//		GL.Vertex3(1, 1, 0);
//		GL.Color(Color(0, 0, 0, 0.5));
//		GL.Vertex3(0, 0, 0);
//		GL.Vertex3(0, 1, 0);
//		GL.Vertex3(1, 0, 0);
//		GL.Vertex3(1, 1, 0);
//		GL.End();
	}
}
