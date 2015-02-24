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
    
	public GameObject bulletPrefab;
	public static GameObject BulletPrefab {
		get {
			return Instance.bulletPrefab;
		}
	}

	public static void Spawn(int bulletId, int playerIdSource, int unitIdSource, int playerIdTarget, int unitIdTarget) {
		//TODO shoot moi ca Kevin
	}
	
	public static Bullet Spawn(TroopBase source, TroopBase target) {
		Bullet spawned = hObjectPool.Instance.Spawn(BulletPrefab, source.transform.position, source.transform.rotation).GetComponent<Bullet>();
		
		spawned.source = source;
		spawned.target = target;
		
		return spawned;
	}
	
	public static void Despawn(Bullet bullet) {
		hObjectPool.Instance.Despawn(bullet.gameObject);
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

	public static void MoveBullet(int bulletPlayerId, int bulletId, Vector3 position, Vector3 velocity) {
		// TODO kevin touche moi par la
	}

	public static void RemoveBullet(int bulletPlayerId, int bulletId) {
		//TODO Retire moi de ta vie kevin! c'est FINI!
	}
}

