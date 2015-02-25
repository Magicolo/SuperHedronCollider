using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

[System.Serializable]
public class TroopGroup {

	public int playerId;
	public int groupId;
	
	readonly Dictionary<int, TroopBase> idTroopDict = new Dictionary<int, TroopBase>();
	Rect zone;
	
	public void UpdateZone() {
		
	}
	
	public void AddTroop(TroopBase troop) {
		idTroopDict[troop.id] = troop;
	}
	
	public void RemoveTroop(int troopId) {
		
	}
	
	public void RemoveTroop(TroopBase troop) {
		RemoveTroop(troop.id);
	}
}

