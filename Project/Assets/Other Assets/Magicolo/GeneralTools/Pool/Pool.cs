using UnityEngine;
using System.Collections;
using Magicolo;
using Magicolo.GeneralTools;

[ExecuteInEditMode]
public class Pool : MonoBehaviour {

	static Pool instance;
	static Pool Instance {
		get {
			if (instance == null) {
				instance = FindObjectOfType<Pool>();
			}
			
			return instance;
		}
	}
	
	public PoolPrefabManager prefabManager;
	
	public void Initialize() {
		if (SingletonCheck()) {
			return;
		}
	}
	
	public bool SingletonCheck() {
		if (Instance != null && Instance != this) {
			if (!Application.isPlaying) {
				Logger.LogError(string.Format("There can only be one {0}.", GetType().Name));
			}
			
			gameObject.Remove();
				
			return true;
		}
			
		return false;
	}

	void Awake() {
		Initialize();
	}
}
