using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

[System.Serializable]
public class TroopGroupManager {

	public int playerId;
	
	readonly Dictionary<int, TroopGroup> idGroupDict = new Dictionary<int, TroopGroup>();
	int groupCounter;
	
	public TroopGroupManager(int playerId) {
		this.playerId = playerId;
	}
	
	public void UpdateZones() {
		foreach (TroopGroup group in idGroupDict.Values) {
			group.UpdateZones();
		}
	}

	public void UpdateLights() {
		foreach (TroopGroup group in idGroupDict.Values) {
			group.UpdateLights();
		}
	}

	public int CreateGroup(params TroopBase[] troops) {
		groupCounter += 1;
		
		TroopGroup group = new TroopGroup(playerId, groupCounter);
		
		foreach (TroopBase troop in troops) {
			if (troop.groupId != 0) {
				RemoveTroopFromGroup(troop.groupId, troop.id);
			}
			
			group.AddTroop(troop);
		}
		
		idGroupDict[groupCounter] = group;
		
		return groupCounter;
	}

	public void SwitchTroopsToGroup(int groupId, params TroopBase[] troops) {
		if (idGroupDict.ContainsKey(groupId)) {
			foreach (TroopBase troop in troops) {
				if (troop.groupId != 0) {
					RemoveTroopFromGroup(troop.groupId, troop.id);
				}
				
				AddTroopToGroup(groupId, troop);
			}
		}
	}
	
	public void AddTroopToGroup(int groupId, TroopBase troop) {
		if (idGroupDict.ContainsKey(groupId)) {
			idGroupDict[groupId].AddTroop(troop);
		}
	}
	
	public void RemoveTroopFromGroup(int groupId, int troopId) {
		if (idGroupDict.ContainsKey(groupId)) {
			idGroupDict[groupId].RemoveTroop(troopId);
			
			if (idGroupDict[groupId].GetTroopCount() <= 0) {
				idGroupDict.Remove(groupId);
			}
		}
	}
	
	public void RemoveTroopFromGroup(int groupId, TroopBase troop) {
		RemoveTroopFromGroup(groupId, troop.id);
	}

	public TroopBase[] GetTroopsFromGroup(int groupId) {
		if (idGroupDict.ContainsKey(groupId)) {
			return idGroupDict[groupId].GetTroops();
		}
		
		return new TroopBase[0];
	}

	public bool GroupContains(int groupId, int troopId) {
		if (idGroupDict.ContainsKey(groupId)) {
			return idGroupDict[groupId].Contains(troopId);
		}
		
		return false;
	}

	public bool GroupZoneContains(int groupId, Vector3 point) {
		if (idGroupDict.ContainsKey(groupId)) {
			return idGroupDict[groupId].ZoneContains(point, false);
		}
		
		return false;
	}

	public bool GroupZoneContains(int groupId, TroopBase troop) {
		if (idGroupDict.ContainsKey(groupId)) {
			return idGroupDict[groupId].ZoneContains(troop, false);
		}
		
		return false;
	}

	public int[] ContainingGroupZones(Vector3 point) {
		List<int> zoneIds = new List<int>();
		
		foreach (TroopGroup group in idGroupDict.Values) {
			if (GroupZoneContains(group.id, point)) {
				zoneIds.Add(group.id);
			}
		}
		
		return zoneIds.ToArray();
	}
	
	public void MoveGroup(int groupId, Vector3 target) {
		if (idGroupDict.ContainsKey(groupId)) {
			idGroupDict[groupId].Move(target);
		}
	}
}

