using UnityEngine;
using System.Collections;
using Magicolo;

public class OscillateLight : MonoBehaviourExtended {

	public float frequency = 1;
	public float amplitude = 0.5F;
	
	void Update() {
		transform.OscillateLocalPosition(frequency, amplitude, amplitude, "Z");
	}
}
