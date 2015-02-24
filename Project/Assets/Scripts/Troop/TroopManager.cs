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
	
	static readonly Dictionary<int, PlayerTroopManager> playerIdTroopDict = new Dictionary<int, PlayerTroopManager>();
	
	public static TroopBase Spawn(int playerId, int troopId, int troopTypeId, Vector3 position, Quaternion rotation) {
		TroopBase spawned = hObjectPool.Instance.Spawn(TypeIdToPrefab(troopTypeId), position, rotation).GetComponent<TroopBase>();
		
		if (!playerIdTroopDict.ContainsKey(playerId)) {
			playerIdTroopDict[playerId] = new PlayerTroopManager(playerId);
		}
		
		spawned.playerId = playerId;
		spawned.id = troopId;
		playerIdTroopDict[playerId].AddTroop(spawned);
		
		return spawned;
	}
	
	public static TroopBase Spawn(int playerId, int troopId, int troopTypeId, Vector3 position) {
		return Spawn(playerId, troopId, troopTypeId, position, Quaternion.identity);
	}
	
	public static TroopBase Spawn(int playerId, int troopId, int troopTypeId) {
		return Spawn(playerId, troopId, troopTypeId, Vector3.zero, Quaternion.identity);
	}
	
	public static T Spawn<T>(int playerId, int troopId, Vector3 position, Quaternion rotation) where T : TroopBase {
		return (T)Spawn(playerId, troopId, ToTypeId<T>(), position, rotation);
	}
	
	public static T Spawn<T>(int playerId, int troopId, Vector3 position) where T : TroopBase {
		return Spawn<T>(playerId, troopId, position, Quaternion.identity);
	}
	
	public static T Spawn<T>(int playerId, int troopId) where T : TroopBase {
		return Spawn<T>(playerId, troopId, Vector3.zero, Quaternion.identity);
	}
	
	public static void Despawn(TroopBase troop) {
		playerIdTroopDict[troop.playerId].RemoveTroop(troop);
		
		hObjectPool.Instance.Despawn(troop.gameObject);
	}

	public static void Despawn(int playerId, int troopId) {
		Despawn(GetTroop(playerId, troopId));
	}

	public static TroopBase GetTroop(int playerId, int troopId) {
		if (playerIdTroopDict.ContainsKey(playerId)) {
			return playerIdTroopDict[playerId].GetTroop(troopId);
		}
		
		return null;
	}
	
	public static TroopBase[] GetTroops(int playerId) {
		if (playerIdTroopDict.ContainsKey(playerId)) {
			return playerIdTroopDict[playerId].GetTroops();
		}
		
		return new TroopBase[0];
	}
	
	public static int GetTroopCount(int playerId) {
		return GetTroops(playerId).Length;
	}
	
	public static int[] ContainingZones(TroopBase troop) {
		List<int> zoneIds = new List<int>();
		
		foreach (int playerId in playerIdTroopDict.Keys) {
			if (playerId != troop.playerId && ZoneContains(playerId, troop)) {
				zoneIds.Add(playerId);
			}
		}
		
		return zoneIds.ToArray();
	}
	
	public static bool ZoneContains(int playerId, TroopBase troop) {
		if (!playerIdTroopDict.ContainsKey(playerId)) {
			return false;
		}
		
		return playerIdTroopDict[playerId].ZoneContains(troop);
	}
	
	public static TroopBase GetClosestInRangeEnemy(TroopBase troop) {
		TroopBase closestEnemy = null;
		float closestDistance = float.MaxValue;
		
		foreach (int playerId in ContainingZones(troop)) {
			foreach (TroopBase enemyTroop in GetTroops(playerId)) {
				float distance = Vector3.Distance(enemyTroop.transform.position, troop.transform.position);
				
				if (distance <= troop.sightRadius && distance < closestDistance) {
					closestEnemy = enemyTroop;
					closestDistance = distance;
				}
			}
		}
		
		return closestEnemy;
	}

	public static void DamageTroop(int playerId, int troopId, int damage) {
		if (playerIdTroopDict.ContainsKey(playerId)) {
			playerIdTroopDict[playerId].DamageTroop(troopId, damage);
		}
	}

	public static void MoveTroop(int playerId, int troopId, Vector3 target) {
		if (playerIdTroopDict.ContainsKey(playerId)) {
			playerIdTroopDict[playerId].MoveTroop(troopId, target);
		}
	}

	public static void KillTroop(int playerId, int troopId) {
		if (playerIdTroopDict.ContainsKey(playerId)) {
			playerIdTroopDict[playerId].KillTroop(troopId);
		}
	}
	
	public static int ToTypeId<T>() {
		int typeId;
		
		if (typeof(T) == typeof(TroopHexa)) {
			typeId = 0;
		}
		else if (typeof(T) == typeof(TroopIso)) {
			typeId = 1;
		}
		else {
			typeId = 2;
		}
		
		return typeId;
	}
	
	public static GameObject TypeIdToPrefab(int typeId) {
		GameObject prefab;
		
		if (typeId == 0) {
			prefab = HexaTroopPrefab;
		}
		else if (typeId == 1) {
			prefab = IsoTroopPrefab;
		}
		else {
			prefab = TetraTroopPrefab;
		}
		
		return prefab;
	}
	
	void Update() {
		UpdateZones();
	}
	
	void UpdateZones() {
		foreach (PlayerTroopManager playerTroops in playerIdTroopDict.GetValueArray()) {
			playerTroops.UpdateZone();
		}
	}
}
