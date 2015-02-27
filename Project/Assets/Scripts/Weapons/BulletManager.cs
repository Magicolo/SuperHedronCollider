using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

public class BulletManager : MonoBehaviourExtended {

	static BulletManager instance;
	static BulletManager Instance {
		get {
			if (instance == null) {
				instance = FindObjectOfType<BulletManager>();
			}
			return instance;
		}
	}

	public GameObject hexaBulletPrefab;
	public static GameObject HexaBulletPrefab {
		get {
			return Instance.hexaBulletPrefab;
		}
	}
	
	public GameObject iconaBulletPrefab;
	public static GameObject IconaBulletPrefab {
		get {
			return Instance.iconaBulletPrefab;
		}
	}
	
	public GameObject tetraBulletPrefab;
	public static GameObject TetraBulletPrefab {
		get {
			return Instance.tetraBulletPrefab;
		}
	}
	
	static readonly Dictionary<int, PlayerBulletManager> playerIdBulletDict = new Dictionary<int, PlayerBulletManager>();
	
	public static Bullet Spawn<T>(int bulletId, T source, TroopBase target) where T : TroopBase {
		Bullet bullet = hObjectPool.Instance.Spawn(TypeIdToPrefab(ToTypeId<T>()), source.transform.position, Quaternion.identity).GetComponent<Bullet>();
		
		if (!playerIdBulletDict.ContainsKey(source.playerId)) {
			playerIdBulletDict[source.playerId] = new PlayerBulletManager(source.playerId);
		}
		
		bullet.lifeCounter = source.bulletLifeTime;
		bullet.source = source;
		bullet.target = target;
		bullet.id = bulletId;
		playerIdBulletDict[source.playerId].AddBullet(bullet);
			
		return bullet;
	}
	
	public static Bullet Spawn(int bulletId, int playerIdSource, int troopIdSource, int playerIdTarget, int troopIdTarget) {
		return Spawn(bulletId, TroopManager.GetTroop(playerIdSource, troopIdSource), TroopManager.GetTroop(playerIdTarget, troopIdTarget));
	}
	
	public static void Despawn(Bullet bullet) {
		if (playerIdBulletDict.ContainsKey(bullet.source.playerId)) {
			playerIdBulletDict[bullet.source.playerId].RemoveBullet(bullet);
		
			hObjectPool.Instance.Despawn(bullet.gameObject);
		}
	}
	
	public static void Despawn(int playerId, int bulletId) {
		Bullet bullet = GetBullet(playerId, bulletId);
		
		if (bullet != null) {
			Despawn(bullet);
		}
	}

	public static Bullet GetBullet(int playerId, int bulletId) {
		if (playerIdBulletDict.ContainsKey(playerId)) {
			return playerIdBulletDict[playerId].GetBullet(bulletId);
		}
		
		return null;
	}
	
	public static void MoveBullet(int playerId, int bulletId, Vector3 position) {
		if (playerIdBulletDict.ContainsKey(playerId)) {
			playerIdBulletDict[playerId].MoveBullet(bulletId, position);
		}
	}

	public static void KillBullet(int playerId, int bulletId) {
		if (playerIdBulletDict.ContainsKey(playerId)) {
			playerIdBulletDict[playerId].KillBullet(bulletId);
		}
	}

	public static int ToTypeId<T>() where T : TroopBase {
		return TroopManager.ToTypeId<T>();
	}
	
	public static GameObject TypeIdToPrefab(int typeId) {
		GameObject prefab;
		
		if (typeId == 0) {
			prefab = HexaBulletPrefab;
		}
		else if (typeId == 1) {
			prefab = IconaBulletPrefab;
		}
		else {
			prefab = TetraBulletPrefab;
		}
		
		return prefab;
	}
}

