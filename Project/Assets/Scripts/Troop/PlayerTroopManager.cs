using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

[System.Serializable]
public class PlayerTroopManager {

	public int playerId;
	
	readonly Dictionary<int, TroopBase> idTroopDict = new Dictionary<int, TroopBase>();
	Rect troopZone;
	Rect sightZone;
	
	public PlayerTroopManager(int playerId) {
		this.playerId = playerId;
	}

	public void UpdateZones() {
		float xMinTroop = float.MaxValue;
		float xMaxTroop = float.MinValue;
		float yMinTroop = float.MaxValue;
		float yMaxTroop = float.MinValue;
		
		float xMinSight = float.MaxValue;
		float xMaxSight = float.MinValue;
		float yMinSight = float.MaxValue;
		float yMaxSight = float.MinValue;
			
		foreach (TroopBase troop in GetTroops()) {
			Rect troopRect = troop.GetRect();
			Rect sightRect = troop.GetSightRect();
			
			if (troopRect.xMin < xMinTroop) {
				xMinTroop = troopRect.xMin;
			}
		
			if (troopRect.xMax > xMaxTroop) {
				xMaxTroop = troopRect.xMax;
			}
		
			if (troopRect.yMin < yMinTroop) {
				yMinTroop = troopRect.yMin;
			}
		
			if (troopRect.yMax > yMaxTroop) {
				yMaxTroop = troopRect.yMax;
			}
			
			if (sightRect.xMin < xMinSight) {
				xMinSight = sightRect.xMin;
			}
		
			if (sightRect.xMax > xMaxSight) {
				xMaxSight = sightRect.xMax;
			}
		
			if (sightRect.yMin < yMinSight) {
				yMinSight = sightRect.yMin;
			}
		
			if (sightRect.yMax > yMaxSight) {
				yMaxSight = sightRect.yMax;
			}
		}
			
		troopZone = Rect.MinMaxRect(xMinTroop, yMinTroop, xMaxTroop, yMaxTroop);
		sightZone = Rect.MinMaxRect(xMinSight, yMinSight, xMaxSight, yMaxSight);
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

	public bool ZoneContains(TroopBase troop, bool useSightRadius = true) {
		return useSightRadius ? sightZone.Intersects(troop.GetSightRect()) : troopZone.Intersects(troop.GetRect());
	}

	public void DamageTroop(int troopId, float damage) {
		TroopBase troop = GetTroop(troopId);
		
		if (troop != null) {
			troop.Damage(damage);
		}
	}

	public void SetTroopTarget(int troopId, Vector3 target) {
		TroopBase troop = GetTroop(troopId);
		
		if (troop != null) {
			troop.SetTarget(target);
		}
	}

	public void MoveTroop(int troopId, Vector3 position) {
		TroopBase troop = GetTroop(troopId);
		
		if (troop != null) {
			troop.Move(position);
		}
	}

	public void KillTroop(int troopId) {
		TroopBase troop = GetTroop(troopId);
		
		if (troop != null) {
			troop.Kill();
		}
	}

	public void FadeTroopLight(int troopId, float intensity, float range, bool enabled) {
		TroopBase troop = GetTroop(troopId);
		
		if (troop != null) {
			troop.FadeLight(intensity, range, enabled);
		}
	}
}

