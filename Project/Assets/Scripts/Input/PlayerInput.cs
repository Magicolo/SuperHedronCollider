using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

public class PlayerInput : StateLayer {
	
	public bool debug;
	public int mouseSelectButton = 0;
	public int mouseActionButton = 1;
	public int mouseMoveButton = 2;
	public List<TroopBase> selectedTroops = new List<TroopBase>();
	
	public override void OnUpdate() {
		base.OnUpdate();
		
		if (debug && Input.GetKey(KeyCode.Space)) {
			if (!NetworkController.instance.isConnected) {
				TroopManager.Spawn<TroopHexa>(new []{ NetworkController.CurrentPlayerId, 100 }.GetRandom(), Random.Range(0, int.MaxValue));
			}
			else if (NetworkController.instance.clientController.playerId == 1) {
				NetworkController.instance.clientController.spawnUnit(0, new Vector3(-250, 0, 0), Quaternion.identity);
			}
			else {
				NetworkController.instance.clientController.spawnUnit(0, new Vector3(250, 0, 0), Quaternion.identity);
			}
			
		}
	}
}
