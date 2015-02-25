using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

[System.Serializable]
public class TroopGroupManager {

	public int playerId;
	
	readonly Dictionary<int, TroopGroup> idGroupDict = new Dictionary<int, TroopGroup>();
	
	public void UpdateZones() {
		foreach (TroopGroup group in idGroupDict.GetValueArray()) {
			group.UpdateZone();
		}
	}
}

