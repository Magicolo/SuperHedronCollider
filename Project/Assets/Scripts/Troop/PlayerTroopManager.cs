using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

[System.Serializable]
public class PlayerTroopManager {

	public int playerId;
	
	readonly Dictionary<int, TroopBase> idTroopDict = new Dictionary<int, TroopBase>();
	Rect zone;
	
	public PlayerTroopManager(int playerId) {
		this.playerId = playerId;
	}

	public void UpdateZone() {
		float xMin = float.MaxValue;
		float xMax = float.MinValue;
		float yMin = float.MaxValue;
		float yMax = float.MinValue;
			
		foreach (TroopBase troop in GetTroops()) {
			if (troop.transform.position.x - troop.sightRadius < xMin) {
				xMin = troop.transform.position.x - troop.sightRadius;
			}
		
			if (troop.transform.position.x + troop.sightRadius > xMax) {
				xMax = troop.transform.position.x + troop.sightRadius;
			}
		
			if (troop.transform.position.z - troop.sightRadius < yMin) {
				yMin = troop.transform.position.z - troop.sightRadius;
			}
		
			if (troop.transform.position.z + troop.sightRadius > yMax) {
				yMax = troop.transform.position.z + troop.sightRadius;
			}
		}
			
		zone = Rect.MinMaxRect(xMin, yMin, xMax, yMax);
	}

	public TroopBase GetTroop(int troopId) {
		if (idTroopDict.ContainsKey(troopId)) {
			return idTroopDict[troopId];
		}
		
		return null;
	}
	
	public TroopBase[] GetTroops() {
		return idTroopDict.GetValueArray();
	}
	
	public void AddTroop(TroopBase troop) {
		idTroopDict[troop.id] = troop;
	}
	
	public void RemoveTroop(int troopId) {
		idTroopDict.Remove(troopId);
	}
	
	public void RemoveTroop(TroopBase troop) {
		RemoveTroop(troop.id);
	}

	public bool ZoneContains(TroopBase troop) {
		Rect troopRect = Rect.MinMaxRect(troop.transform.position.x - troop.sightRadius, troop.transform.position.z - troop.sightRadius, troop.transform.position.x + troop.sightRadius, troop.transform.position.z + troop.sightRadius);
		
		return zone.Intersects(troopRect);
	}

	public void DamageTroop(int troopId, float damage) {
		TroopBase troop = GetTroop(troopId);
		
		if (troop != null) {
			troop.Damage(damage);
		}
	}

	public void MoveTroop(int troopId, Vector3 position, Vector3 target) {
		TroopBase troop = GetTroop(troopId);
		
		if (troop != null) {
			troop.Move(position, target);
		}
	}

	public void KillTroop(int troopId) {
		TroopBase troop = GetTroop(troopId);
		
		if (troop != null) {
			troop.Kill();
		}
	}
}

