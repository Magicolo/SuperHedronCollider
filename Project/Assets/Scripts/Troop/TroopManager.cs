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
	
	static readonly Dictionary<int, List<TroopBase>> troops = new Dictionary<int, List<TroopBase>>();
	
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
		if (troops.ContainsKey(playerId)) {
			return troops[playerId].Count;
		}
		
		return 0;
	}
}
