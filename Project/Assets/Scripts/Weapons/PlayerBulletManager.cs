using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

[System.Serializable]
public class PlayerBulletManager {

	public int playerId;
    
	readonly Dictionary<int, Bullet> idBulletDict = new Dictionary<int, Bullet>();
	
	public PlayerBulletManager(int playerId) {
		this.playerId = playerId;
	}

	public Bullet GetBullet(int bulletId) {
		if (idBulletDict.ContainsKey(bulletId)) {
			return idBulletDict[bulletId];
		}
		
		return null;
	}
	
	public Bullet[] GetBullets() {
		return idBulletDict.GetValueArray();
	}
	
	public void AddBullet(Bullet bullet) {
		idBulletDict[bullet.id] = bullet;
	}
	
	public void RemoveBullet(int bulletId) {
		idBulletDict.Remove(bulletId);
	}
	
	public void RemoveBullet(Bullet bullet) {
		RemoveBullet(bullet.id);
	}

	public void MoveBullet(int bulletId, Vector3 position) {
		Bullet bullet = GetBullet(bulletId);
		
		if (bullet != null) {
			bullet.Move(position);
		}
	}

	public void KillBullet(int bulletId) {
		Bullet bullet = GetBullet(bulletId);
		
		if (bullet != null) {
			bullet.Kill();
		}
	}

}

