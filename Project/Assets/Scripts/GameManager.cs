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
	
	Vector3 startCamera;
	Vector3 endCamera;
	float countDownPreparationCameraMovement;
	bool InPrepareStart;

	public void PrepareStart() {
		startCamera = NetworkController.instance.currentPlayer.cameraStartingLocation;
		endCamera = NetworkController.instance.currentPlayer.superHedronCollider.transform.position;
		endCamera += new Vector3(0,75,0);
		InPrepareStart = true;
	}
	
	public void Start() {
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

