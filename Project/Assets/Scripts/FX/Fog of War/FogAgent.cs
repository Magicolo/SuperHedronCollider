using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

[System.Serializable]
public class FogAgent {

	public Transform transform;
	[Min] public float sightRadius = 10;
	public bool clearsFog = true;
	
	public FogAgent(Transform transform, float sightRadius, bool clearsFog) {
		this.transform = transform;
		this.sightRadius = sightRadius;
		this.clearsFog = clearsFog;
	}
	
	public FogAgent(Transform transform, float sightRadius) {
		this.transform = transform;
		this.sightRadius = sightRadius;
	}
}

