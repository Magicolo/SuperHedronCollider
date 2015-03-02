using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

public class GameManager : MonoBehaviourExtended {

	static GameManager instance;
	public static GameManager Instance {
		get {
			if (instance == null) {
				instance = FindObjectOfType<GameManager>();
			}
			return instance;
		}
	}

	public bool debug;
		public static bool Debug {
			get {
				return Instance.debug;
			}
		}

    
	
	Vector3 startCamera;
	Vector3 endCamera;
	float countDownPreparationCameraMovement;
	bool InPrepareStart;
	
	List<FogAgent> fogAgentToRemove = new List<FogAgent>();

	public void PrepareStart() {
		startCamera = NetworkController.instance.currentPlayer.cameraStartingLocation;
		endCamera = NetworkController.instance.currentPlayer.superHedronCollider.transform.position;
		endCamera += new Vector3(0,75,0);
		
		MapSettings mapSettings = NetworkController.instance.currentMap;
		for (int playerId = 0; playerId < mapSettings.players.Length; playerId++) {
			MapPlayerSettings playerSettings = mapSettings.players[playerId];
			makeAgent(playerSettings.superHedronCollider.transform, 40, playerId);
			foreach (var spawner in playerSettings.spawnners) {
				makeAgent(spawner.transform, 20, playerId);
			}
		}
		
		InPrepareStart = true;
	}

	void makeAgent(Transform agentTranform, int radius, int playerId) {
		FogAgent fogAgent = new FogAgent(agentTranform,radius);
		if(playerId != NetworkController.instance.clientController.playerId){
			fogAgentToRemove.Add(fogAgent);
		}
		References.Fow.AddAgent(fogAgent);
	}
	
	public void Start() {
		References.Fow.RemoveAgent(fogAgentToRemove.ToArray());
		fogAgentToRemove.Clear();
		InPrepareStart = false;
		//TODO activate Spawnners
		//Time.timeScale = 1;
	}
	
	public void Stop() {
		
	}
	
	void Awake(){
		Stop();
	}
	
	void Update(){
		if(InPrepareStart){
			countDownPreparationCameraMovement += Time.deltaTime;
			Camera.main.transform.position = Vector3.Lerp(startCamera, endCamera, countDownPreparationCameraMovement/5);
		}
	}
}

