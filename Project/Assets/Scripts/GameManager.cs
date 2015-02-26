using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

public class GameManager : MonoBehaviourExtended {

	static GameManager instance;
	static GameManager Instance {
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
	
	public static void Start() {
		Time.timeScale = 1;
	}
	
	public static void Stop() {
		Time.timeScale = 0;
	}
	
	void Awake(){
		Stop();
	}
}

