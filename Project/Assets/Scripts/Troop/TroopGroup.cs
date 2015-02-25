using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

[System.Serializable]
public class TroopGroup {

	public int playerId;
	public int id;
	
	readonly Dictionary<int, TroopBase> idTroopDict = new Dictionary<int, TroopBase>();
	Rect troopZone;
	Rect sightZone;
	
	public TroopGroup(int playerId, int groupId) {
		this.playerId = playerId;
		this.id = groupId;
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
	
	public void UpdateLights() {
		
	}
	
	public void AddTroop(TroopBase troop) {
		troop.groupId = id;
		idTroopDict[troop.id] = troop;
	}
	
	public void RemoveTroop(int troopId) {
		RemoveTroop(idTroopDict[troopId]);
	}
	
	public void RemoveTroop(TroopBase troop) {
		troop.groupId = 0;
		idTroopDict.Remove(troop.id);
	}

	public TroopBase[] GetTroops() {
		return idTroopDict.GetValueArray();
	}

	public int GetTroopCount() {
		return idTroopDict.Count;
	}
	
	public bool Contains(int troopId) {
		return idTroopDict.GetKeyArray().Contains(troopId);
	}

	public bool ZoneContains(Vector3 point, bool useSightRadius = true) {
		return useSightRadius ? sightZone.Contains(new Vector3(point.x, point.z, point.y)) : troopZone.Contains(new Vector3(point.x, point.z, point.y));
	}
	
	public bool ZoneContains(TroopBase troop, bool useSightRadius = true) {
		return useSightRadius ? sightZone.Intersects(troop.GetSightRect()) : troopZone.Intersects(troop.GetRect());
	}
	
	public void Move(Vector3 target) {
		int troopCounter = 0;
		int segmentLength = 1;
		Vector3 lastPosition = target;
		Vector3 currentDirection = Vector3.forward;
		TroopBase[] troops = GetTroops();
			
		while (troopCounter < troops.Length) {
			for (int i = 0; i < segmentLength / 2;) {
				TroopBase troop = troops[troopCounter];
					
				if (troop.gameObject.activeInHierarchy && troop.Selected) {
					troop.Target = lastPosition;
					lastPosition += currentDirection * troop.radius;
					i++;
				}
					
				troopCounter += 1;
					
				if (troopCounter >= troops.Length) {
					break;
				}
			}
				
			currentDirection = currentDirection.Rotate(90, Vector3.up);
			segmentLength += 1;
		}
	}
}

