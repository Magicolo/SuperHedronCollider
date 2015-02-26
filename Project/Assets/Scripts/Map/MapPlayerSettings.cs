using UnityEngine;
using System.Collections;

[System.Serializable]
public class MapPlayerSettings  {

	public GameObject superHedronCollider;
	public GameObject[] spawnners;
	
	public void setUpFor(int playerId){
		foreach (var spawnner in spawnners) {
			spawnner.GetComponentInChildren<Light>().color = TeamStaticStuff.getColorForTeam(playerId);
		}
		superHedronCollider.GetComponentInChildren<Light>().color = TeamStaticStuff.getColorForTeam(playerId);
	}
	
	public void imPlayer(int playerId){
		
	}
}
