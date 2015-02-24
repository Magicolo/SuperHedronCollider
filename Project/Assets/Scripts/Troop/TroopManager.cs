using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

public class TroopManager : MonoBehaviourExtended {

	static TroopManager instance;
	static TroopManager Instance {
		get {
			if (instance == null) {
				instance = FindObjectOfType<TroopManager>();
			}
			return instance;
		}
	}
    
	static int idCounter;
	
	public GameObject hexaTroopPrefab;
	public static GameObject HexaTroopPrefab {
		get {
			return Instance.hexaTroopPrefab;
		}
	}
	
	public GameObject isoTroopPrefab;
	public static GameObject IsoTroopPrefab {
		get {
			return Instance.isoTroopPrefab;
		}
	}
	
	public GameObject tetraTroopPrefab;
	public static GameObject TetraTroopPrefab {
		get {
			return Instance.tetraTroopPrefab;
		}
	}
	
	static readonly Dictionary<int, List<TroopBase>> troops = new Dictionary<int, List<TroopBase>>();
	static readonly Dictionary<int, Rect> zones = new Dictionary<int, Rect>();
	
	public static void Spawn(int playerId, int troopTypeId, int troopId, Vector3 position, Quaternion rotation){
		//TODO todo
	}
	
	public static T Spawn<T>(int playerId, Vector3 position, Quaternion rotation) where T : TroopBase {
		GameObject toSpawn;
		
		if (typeof(T) == typeof(TroopHexa)) {
			toSpawn = HexaTroopPrefab;
		}
		else if (typeof(T) == typeof(TroopIso)) {
			toSpawn = IsoTroopPrefab;
		}
		else {
			toSpawn = TetraTroopPrefab;
		}
		
		T spawned = hObjectPool.Instance.Spawn(toSpawn, position, rotation).GetComponent<T>();
		
		if (!troops.ContainsKey(playerId)) {
			troops[playerId] = new List<TroopBase>();
		}
		
		spawned.playerId = playerId;
		troops[playerId].Add(spawned);
		
		return spawned;
	}
	
	public static T Spawn<T>(int playerId, Vector3 position) where T : TroopBase {
		return Spawn<T>(playerId, position, Quaternion.identity);
	}
	
	public static T Spawn<T>(int playerId) where T : TroopBase {
		return Spawn<T>(playerId, Vector3.zero, Quaternion.identity);
	}

	public static void Kill(int troopId){
		// TODO kill this unit
	}
	
	public static void Despawn(TroopBase troop, int playerId) {
		if (!troops.ContainsKey(playerId)) {
			troops[playerId] = new List<TroopBase>();
		}
		troops[playerId].Remove(troop);
		
		hObjectPool.Instance.Despawn(troop.gameObject);
	}

	public static TroopBase[] GetTroops(int playerId) {
		if (troops.ContainsKey(playerId)) {
			return troops[playerId].ToArray();
		}
		
		return new TroopBase[0];
	}
	
	public static int GetTroopCount(int playerId) {
		return GetTroops(playerId).Length;
	}
	
	public static int[] ContainingZones(TroopBase troop) {
		List<int> zoneIds = new List<int>();
		
		foreach (int playerId in troops.Keys) {
			if (playerId != troop.playerId && ZoneContains(playerId, troop)) {
				zoneIds.Add(playerId);
			}
		}
		
		return zoneIds.ToArray();
	}
	
	public static bool ZoneContains(int playerId, TroopBase troop) {
		if (!zones.ContainsKey(playerId)) {
			return false;
		}
		
		Rect zone = zones[playerId];
		Rect troopRect = Rect.MinMaxRect(troop.transform.position.x - troop.sightRadius, troop.transform.position.z - troop.sightRadius, troop.transform.position.x + troop.sightRadius, troop.transform.position.z + troop.sightRadius);
		
		return zone.Intersects(troopRect);
	}
	
	public static TroopBase GetClosestInRangeEnemy(TroopBase troop) {
		TroopBase closestEnemy = null;
		float closestDistance = float.MaxValue;
		
		foreach (int playerId in ContainingZones(troop)) {
			foreach (TroopBase enemyTroop in troops[playerId]) {
				float distance = Vector3.Distance(enemyTroop.transform.position, troop.transform.position);
				
				if (distance <= troop.sightRadius && distance < closestDistance) {
					closestEnemy = enemyTroop;
					closestDistance = distance;
				}
			}
		}
		
		return closestEnemy;
	}
	
	void Update() {
		UpdateZones();
	}
	
	void UpdateZones() {
		foreach (int playerId in troops.Keys) {
			float xMin = float.MaxValue;
			float xMax = float.MinValue;
			float yMin = float.MaxValue;
			float yMax = float.MinValue;
			
			foreach (TroopBase troop in GetTroops(playerId)) {
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
			
			zones[playerId] = Rect.MinMaxRect(xMin, yMin, xMax, yMax);
		}
	}
}
