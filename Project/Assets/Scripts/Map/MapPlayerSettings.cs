using UnityEngine;
using System.Collections;

[System.Serializable]
public class MapPlayerSettings  {

	public Vector3 cameraStartingLocation;
	public GameObject superHedronCollider;
	public GameObject[] spawnners;
	
	public void setUpFor(int playerId){
		foreach (var spawnner in spawnners) {
			spawnner.GetComponentInChildren<Light>().color = TeamStaticStuff.getColorForTeam(playerId);
		}
		superHedronCollider.GetComponentInChildren<Light>().color = TeamStaticStuff.getColorForTeam(playerId);
	}

	public void setUpMeAsPlayer() {
		Camera.main.transform.position = cameraStartingLocation;
	}
}
