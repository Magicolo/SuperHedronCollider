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
}

