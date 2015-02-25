using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

public class OscillatePosition : MonoBehaviourExtended {

	public float frequency = 1;
	public float amplitude = 0.5F;
	public float center = 0.5F;
	
	void Update() {
		transform.OscillateLocalPosition(frequency, amplitude, center, "Z");
	}
}

