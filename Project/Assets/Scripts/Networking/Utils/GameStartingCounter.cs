using UnityEngine;
using System.Collections;

public class GameStartingCounter : MonoBehaviour {

	public bool startCounting = false;
	
	float time = 5;
	int nextNumber = 5;
	
	void Update () {
		if(startCounting){
			time -= Time.deltaTime;
			if(nextNumber == 0 && time <= 0){
				networkView.RPC("ClientCenterScreenMessage",RPCMode.All,"Start !!", 2f);
				networkView.RPC("StartGame", RPCMode.All);
				startCounting = false;
			}else if(time <= nextNumber){
				networkView.RPC("ClientCenterScreenMessage",RPCMode.All, "" + nextNumber + " ... ", 0.8f);
				nextNumber--;
				
			}
		}
	}
}
