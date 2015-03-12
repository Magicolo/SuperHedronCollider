using UnityEngine;
using System.Collections;


public class TrooperPooper : MonoBehaviour {
	public int type;
	
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void Spawn (int playerId) {
		if (NetworkController.instance && playerId == NetworkController.CurrentPlayerId){
			NetworkController.instance.clientController.spawnUnit(type, transform.position, Quaternion.identity);
		}
	}
}
