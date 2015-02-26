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
	static readonly Dictionary<int, TroopGroupManager> playerIdGroupDict = new Dictionary<int, TroopGroupManager>();
	
	public static TroopBase Spawn(int playerId, int troopId, int troopTypeId, Vector3 position, Quaternion rotation) {
		TroopBase troop = hObjectPool.Instance.Spawn(TypeIdToPrefab(troopTypeId), position, rotation).GetComponent<TroopBase>();
		
		if (!playerIdTroopDict.ContainsKey(playerId)) {
			playerIdTroopDict[playerId] = new PlayerTroopManager(playerId);
		}
		
		troop.childLight.enabled = false;
		troop.gameObject.layer = playerId == NetworkController.CurrentPlayerId ? 9 : 10;
		troop.playerId = playerId;
		troop.id = troopId;
		troop.childLight.color = TeamStaticStuff.getColorForTeam(playerId);
		playerIdTroopDict[playerId].AddTroop(troop);
		
		return troop;
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
		if (playerIdTroopDict.ContainsKey(troop.playerId)) {
			playerIdTroopDict[troop.playerId].RemoveTroop(troop);
		
			hObjectPool.Instance.Despawn(troop.gameObject);
		}
	}

	public static void Despawn(int playerId, int troopId) {
		TroopBase troop = GetTroop(playerId, troopId);
		
		if (troop != null) {
			Despawn(troop);
		}
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
	
	public static int[] ContainingTroopZones(TroopBase troop, bool includeOwn) {
		List<int> zoneIds = new List<int>();
		
		foreach (int playerId in playerIdTroopDict.Keys) {
			if ((includeOwn || playerId != troop.playerId) && TroopZoneContains(playerId, troop)) {
				zoneIds.Add(playerId);
			}
		}
		
		return zoneIds.ToArray();
	}
	
	public static int[] ContainingTroopZones(TroopBase troop) {
		return ContainingTroopZones(troop, false);
	}
	
	public static bool TroopZoneContains(int playerId, TroopBase troop) {
		if (!playerIdTroopDict.ContainsKey(playerId)) {
			return false;
		}
		
		return playerIdTroopDict[playerId].ZoneContains(troop, true);
	}
	
	public static TroopBase[] GetInRangeAllies(TroopBase troop) {
		return GetInRangeTroops(troop, troop.playerId);
	}

	public static TroopBase GetClosestInRangeAlly(TroopBase troop) {
		return GetClosestInRangeTroop(troop, troop.playerId);
	}

	public static TroopBase[] GetInRangeEnemies(TroopBase troop) {
		return GetInRangeTroops(troop, ContainingTroopZones(troop));
	}

	public static TroopBase GetClosestInRangeEnemy(TroopBase troop) {
		return GetClosestInRangeTroop(troop, ContainingTroopZones(troop));
	}

	public static TroopBase[] GetInRangeTroops(TroopBase troop, params int[] playerIds) {
		List<TroopBase> inRangeTroops = new List<TroopBase>();
		
		foreach (int playerId in playerIds) {
			foreach (TroopBase otherTroop in GetTroops(playerId)) {
				float distance = Vector3.Distance(otherTroop.transform.position, troop.transform.position);
				
				if (distance <= troop.sightRadius) {
					inRangeTroops.Add(otherTroop);
				}
			}
		}
		
		return inRangeTroops.ToArray();
	}

	public static TroopBase GetClosestInRangeTroop(TroopBase troop, params int[] playerIds) {
		TroopBase closestTroop = null;
		float closestDistance = float.MaxValue;
		
		foreach (int playerId in playerIds) {
			foreach (TroopBase otherTroop in GetTroops(playerId)) {
				float distance = Vector3.Distance(troop.transform.position, otherTroop.transform.position);
				
				if (distance <= troop.sightRadius && distance < closestDistance) {
					closestTroop = otherTroop;
					closestDistance = distance;
				}
			}
		}
		
		return closestTroop;
	}

	public static void DamageTroop(int playerId, int troopId, float damage) {
		if (playerIdTroopDict.ContainsKey(playerId)) {
			playerIdTroopDict[playerId].DamageTroop(troopId, damage);
		}
	}

	public static void SetTroopTarget(int playerId, int troopId, Vector3 target) {
		if (playerIdTroopDict.ContainsKey(playerId)) {
			playerIdTroopDict[playerId].SetTroopTarget(troopId, target);
		}
	}
	
	public static void MoveTroop(int playerId, int troopId, Vector3 position) {
		if (playerIdTroopDict.ContainsKey(playerId)) {
			playerIdTroopDict[playerId].MoveTroop(troopId, position);
		}
	}

	public static void KillTroop(int playerId, int troopId) {
		if (playerIdTroopDict.ContainsKey(playerId)) {
			playerIdTroopDict[playerId].KillTroop(troopId);
		}
	}
	
	public static void FadeTroopLight(int playerId, int troopId, float intensity, float range, bool enabled) {
		if (playerIdTroopDict.ContainsKey(playerId)) {
			playerIdTroopDict[playerId].FadeTroopLight(troopId, intensity, range, enabled);
		}
	}
	
	public static int CreateGroup(int playerId, params TroopBase[] troops) {
		if (!playerIdGroupDict.ContainsKey(playerId)) {
			playerIdGroupDict[playerId] = new TroopGroupManager(playerId);
		}
		
		return playerIdGroupDict[playerId].CreateGroup(troops);
	}
		
	public static void SwitchTroopsToGroup(int playerId, int groupId, params TroopBase[] troops) {
		if (playerIdGroupDict.ContainsKey(playerId)) {
			playerIdGroupDict[playerId].SwitchTroopsToGroup(groupId, troops);
		}
	}
	
	public static void AddTroopToGroup(int playerId, int groupId, TroopBase troop) {
		if (playerIdGroupDict.ContainsKey(playerId)) {
			playerIdGroupDict[playerId].AddTroopToGroup(groupId, troop);
		}
	}
	
	public static void RemoveTroopFromGroup(int playerId, int groupId, int troopId) {
		if (playerIdGroupDict.ContainsKey(playerId)) {
			playerIdGroupDict[playerId].RemoveTroopFromGroup(groupId, troopId);
		}
	}
	
	public static void RemoveTroopFromGroup(int playerId, int groupId, TroopBase troop) {
		RemoveTroopFromGroup(playerId, groupId, troop.id);
	}

	public static void RemoveTroopFromGroup(TroopBase troop) {
		RemoveTroopFromGroup(troop.playerId, troop.groupId, troop);
	}
	
	public static TroopBase[] GetTroopsFromGroup(int playerId, int groupId) {
		if (playerIdGroupDict.ContainsKey(playerId)) {
			return playerIdGroupDict[playerId].GetTroopsFromGroup(groupId);
		}
		
		return new TroopBase[0];
	}

	public static int[] ContainingGroupZones(int playerId, Vector3 point) {
		if (playerIdGroupDict.ContainsKey(playerId)) {
			return playerIdGroupDict[playerId].ContainingGroupZones(point);
		}
		
		return new int[0];
	}
	
	public static bool GroupContains(int playerId, int groupId, int troopId) {
		if (playerIdGroupDict.ContainsKey(playerId)) {
			return playerIdGroupDict[playerId].GroupContains(groupId, troopId);
		}
		
		return false;
	}
	
	public static bool GroupContains(int playerId, int groupId, TroopBase troop) {
		return GroupContains(playerId, groupId, troop.id);
	}
	
	public static bool GroupZoneContains(int playerId, int groupId, TroopBase troop) {
		if (playerIdGroupDict.ContainsKey(playerId)) {
			return playerIdGroupDict[playerId].GroupZoneContains(groupId, troop);
		}
		
		return false;
	}
	
	public static void MoveGroup(int playerId, int groupId, Vector3 target) {
		if (playerIdGroupDict.ContainsKey(playerId)) {
			playerIdGroupDict[playerId].MoveGroup(groupId, target);
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
	
	public static bool HasAdvantage(TroopBase source, TroopBase target) {
		bool hasAdvantage;
		
		if (source is TroopHexa) {
			hasAdvantage = target is TroopIso;
		}
		else if (source is TroopIso) {
			hasAdvantage = target is TroopTetra;
		}
		else {
			hasAdvantage = target is TroopHexa;
		}
		
		return hasAdvantage;
	}
	
	void Update() {
		foreach (PlayerTroopManager troops in playerIdTroopDict.Values) {
			troops.UpdateZones();
		}
		
		foreach (TroopGroupManager groups in playerIdGroupDict.Values) {
			groups.UpdateZones();
			groups.UpdateLights();
		}
	}
}
