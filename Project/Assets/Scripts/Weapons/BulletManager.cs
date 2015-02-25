﻿using UnityEngine;
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
    
	public GameObject bulletPrefab;
	public static GameObject BulletPrefab {
		get {
			return Instance.bulletPrefab;
		}
	}

	static readonly Dictionary<int, PlayerBulletManager> playerIdBulletDict = new Dictionary<int, PlayerBulletManager>();
	
	public static Bullet Spawn(int bulletId, TroopBase source, TroopBase target) {
		Bullet bullet = hObjectPool.Instance.Spawn(BulletPrefab, source.transform.position, Quaternion.identity).GetComponent<Bullet>();
		
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
}

