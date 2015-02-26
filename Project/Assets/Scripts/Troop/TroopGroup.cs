using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

[System.Serializable]
public class TroopGroup {

	public int playerId;
	public int id;
	
	const float lightFadeSpeed = 1;
	const float lightGrowFactor = 0.5F;
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
		
		if (Application.isEditor) {
			Debug.DrawRay(new Vector3(troopZone.xMin, 1, troopZone.yMin), Vector3.forward * troopZone.height, Color.cyan);
			Debug.DrawRay(new Vector3(troopZone.xMin, 1, troopZone.yMin), Vector3.right * troopZone.width, Color.cyan);
			Debug.DrawRay(new Vector3(troopZone.xMax, 1, troopZone.yMax), Vector3.back * troopZone.height, Color.cyan);
			Debug.DrawRay(new Vector3(troopZone.xMax, 1, troopZone.yMax), Vector3.left * troopZone.width, Color.cyan);
		
			Debug.DrawRay(new Vector3(sightZone.xMin, 1, sightZone.yMin), Vector3.forward * sightZone.height, Color.magenta);
			Debug.DrawRay(new Vector3(sightZone.xMin, 1, sightZone.yMin), Vector3.right * sightZone.width, Color.magenta);
			Debug.DrawRay(new Vector3(sightZone.xMax, 1, sightZone.yMax), Vector3.back * sightZone.height, Color.magenta);
			Debug.DrawRay(new Vector3(sightZone.xMax, 1, sightZone.yMax), Vector3.left * sightZone.width, Color.magenta);
		}
	}
	
	public void UpdateLights() {
		TroopBase[] troops = GetTroops();
		TroopBase centerTroop = troops[0];
		float centerIntensityTarget = 0;
		float centerRangeTarget = 0;
				
		for (int i = 1; i < troops.Length; i++) {
			TroopBase troop = troops[i];
			float distanceToCenter = Vector3.Distance(troop.transform.position, centerTroop.transform.position);
			float intensityTarget = -1;
			float rangeTarget = -1;
				
			troop.childLight.intensity = Mathf.Lerp(troop.childLight.intensity, intensityTarget, Time.deltaTime * lightFadeSpeed);
			troop.childLight.range = Mathf.Lerp(troop.childLight.range, rangeTarget, Time.deltaTime * lightFadeSpeed);
			troop.childLight.enabled = troop.childLight.intensity > 0 && troop.childLight.range > 0;
			
			centerIntensityTarget += Mathf.Clamp(1 - distanceToCenter / troop.sightRadius, 0, 1) * troop.lightIntensity * lightGrowFactor;
			centerRangeTarget += Mathf.Clamp(1 - distanceToCenter / troop.sightRadius, 0, 1) * troop.lightRange * lightGrowFactor;
		}
		
		centerTroop.childLight.intensity = Mathf.Lerp(centerTroop.childLight.intensity, centerIntensityTarget.Pow(0.75), Time.deltaTime * lightFadeSpeed * 5);
		centerTroop.childLight.range = Mathf.Lerp(centerTroop.childLight.range, centerRangeTarget.Pow(0.75), Time.deltaTime * lightFadeSpeed * 5);
		centerTroop.childLight.enabled = centerTroop.childLight.intensity > 0 && centerTroop.childLight.range > 0;
	}
	
	public void AddTroop(TroopBase troop) {
		troop.groupId = id;
		idTroopDict[troop.id] = troop;
	}
	
	public void RemoveTroop(int troopId) {
		if (idTroopDict.ContainsKey(troopId)) {
			RemoveTroop(idTroopDict[troopId]);
		}
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

	public bool ZoneContains(Vector3 point, bool useSightRadius = false) {
		return useSightRadius ? sightZone.Contains(new Vector3(point.x, point.z, point.y)) : troopZone.Contains(new Vector3(point.x, point.z, point.y));
	}
	
	public bool ZoneContains(TroopBase troop, bool useSightRadius = false) {
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
					
				if (troop.gameObject.activeInHierarchy) {
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

