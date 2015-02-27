using UnityEngine;
using System.Collections;

public enum TroopTypes {
	hexa = 0,
	icosa = 1,
	tetra = 2,
}
public class TrooperPooper : MonoBehaviour {
	public TroopTypes type;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void Spawn () {
		if (NetworkController.instance){
			NetworkController.instance.clientController.spawnUnit((int) type, new Vector3(50, 0, 0), Quaternion.identity);
		} else {
			Debug.LogWarning("There's no network controller so I'm just gonna say this.");
		}
	}
}
