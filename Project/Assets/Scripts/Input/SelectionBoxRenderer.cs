using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

public class SelectionBoxRenderer : MonoBehaviourExtended {

	public static bool draw;
	public static Vector3[] points = new Vector3[0];
	
	void OnPostRender() {
		if (!draw) {
			return;
		}
		
		GL.Begin(GL.QUADS);
		GL.Color(Color.green);
        
		foreach (Vector3 point in points) {
			GL.Vertex3(point.x, point.y, point.z);
		}
		
		GL.End();
	}
}

