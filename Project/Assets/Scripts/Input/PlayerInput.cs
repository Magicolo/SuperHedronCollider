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
//			TroopManager.Spawn<TroopHexa>(new []{ NetworkController.instance.clientController.playerId, 100 }.GetRandom());
		}
	}
}
