using UnityEngine;
using System.Collections;
using Magicolo;

public class OscillateLight : MonoBehaviourExtended {

	public float frequency = 1;
	public float amplitude = 0.5F;
	public float center = 0.5F;
	
	void Update() {
		light.intensity = center + amplitude * Mathf.Sin(frequency * Time.time + GetInstanceID() / 1000);
	}
}
