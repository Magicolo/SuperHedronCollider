using UnityEngine;
using System.Collections;

public class ParticuleManager : MonoBehaviour {

	static ParticuleManager instance;
	static ParticuleManager Instance {
		get {
			if (instance == null) {
				instance = FindObjectOfType<ParticuleManager>();
			}
			return instance;
		}
	}
	
	public GameObject hitParticlePrefab;
	public static GameObject HitParticlePrefab {
		get {
			return Instance.hitParticlePrefab;
		}
	}
}
