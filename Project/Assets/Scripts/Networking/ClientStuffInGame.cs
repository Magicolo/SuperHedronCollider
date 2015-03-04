using UnityEngine;
using System.Collections;

public class ClientStuffInGame : MonoBehaviour {

	
	void Start () {
		
		var net = NetworkController.instance;
		if(net.isConnected){
			net.currentMap.imPlayer(net.clientController.playerId);
			net.currentPlayer = net.currentMap.players[net.clientController.playerId];
			
			net.log("I'm ready");
			net.clientController.sendImReady();
		}else{
			net.log("No connected .. What... ClientSuffInGame pas happy");
		}
	}
	
	
}
