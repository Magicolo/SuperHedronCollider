using UnityEngine;
using System.Collections;

public class ClientStuffInGame : MonoBehaviour {

	
	void Start () {
		
		var net = NetworkController.instance;
		if(net.isConnected){
			net.currentMap.setUpFor(net.clientController.playerId);
			net.clientController.sendImReady();
		}
	}
	
	
}
