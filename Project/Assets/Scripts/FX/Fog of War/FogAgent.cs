using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

[System.Serializable]
public class FogAgent {

	public Transform transform;
	[Min] public float sightRadius = 10;
	[Range(-1, 1)] public float strength = 1;
	
	public Vector3 position { get; set; }
	
	public FogAgent(Transform transform, float sightRadius, float strength) {
		this.transform = transform;
		this.sightRadius = sightRadius;
		this.strength = Mathf.Clamp(strength, -1, 1);
		
		position = transform.position;
	}
	
	public FogAgent(Transform transform, float sightRadius) {
		this.transform = transform;
		this.sightRadius = sightRadius;
		
		position = transform.position;
	}

	public void Update() {
		position = transform.position;
	}
}

