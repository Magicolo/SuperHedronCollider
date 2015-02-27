using UnityEngine;
using System.Collections;

public static class ServerStartSuff {

	public static void StartMoiUnGameDeTest(){
		if(NetworkController.instance.clientController.playerId == 1){
			NetworkController.instance.clientController.spawnUnit(0, new Vector3(-4,0,0), Quaternion.identity);
		}else{
			NetworkController.instance.clientController.spawnUnit(0, new Vector3(4,0,0) , Quaternion.identity);
		}
	}
}
